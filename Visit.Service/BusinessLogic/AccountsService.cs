using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Visit.DataAccess.Auth;
using Visit.DataAccess.Auth.Helpers;
using Visit.DataAccess.EntityFramework;
using Visit.DataAccess.Models;
using Visit.Service.BusinessLogic.BlobStorage;
using Visit.Service.BusinessLogic.Interfaces;
using Visit.Service.Models.Enums;
using Visit.Service.Models.Requests;
using Visit.Service.Models.Responses;

namespace Visit.Service.BusinessLogic
{
    public class AccountsService : IAccountsService
    {
        private readonly ILogger<AccountsService> _logger;
        private readonly IMapper _mapper;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly VisitContext _visitContext;
        private readonly UserManager<User> _userManager;
        private readonly IBlobStorageBusinessLogic _blobStorage;

        public AccountsService(ILogger<AccountsService> logger, IMapper mapper, IJwtFactory jwtFactory, 
            IOptions<JwtIssuerOptions> jwtOptions, VisitContext visitContext, 
            UserManager<User> userManager, IBlobStorageBusinessLogic blobStorage)
        {
            _logger = logger;
            _mapper = mapper;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _visitContext = visitContext;
            _userManager = userManager;
            _blobStorage = blobStorage;
        }
        
        public async Task<CreateUserResponse> RegisterUser(RegisterRequest model)
        {
            var userIdentity = _mapper.Map<User>(model);
            userIdentity.UserName = model.Email;
            var result = await _userManager.CreateAsync(userIdentity, model.Password);
            
            if (!result.Succeeded)
            {
                _logger.LogError($"Could not create user: {result.Errors}");
                return new CreateUserResponse(null, false, result.Errors);
            }

            await _visitContext.SaveChangesAsync();
            
            // Try to get a claim and uplaod image. Prob need some logging/retry handling here
            var claim = await GetClaimsIdentity(model.Email, model.Password);
            
            var token = await LoginUser(new LoginApiRequest()
            {
                UserName = model.Email,
                Password = model.Password
            });
            
            return new CreateUserResponse(token,true,null);
        }
        
        public async Task<JwtToken> LoginUser(LoginApiRequest credentials)
        {
            var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);
            
            if (identity == null)
            {
                return null;
            }
            
            return JsonConvert.DeserializeObject<JwtToken>(
                await Tokens.GenerateJwt(identity, _jwtFactory, identity.Name, _jwtOptions, 
                new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }
        
        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);
            
            User userToVerify;
            if (userName.Contains('@'))
            {
                userToVerify = await _userManager.FindByEmailAsync(userName);
            }
            else
            {
                userToVerify = await _userManager.FindByNameAsync(userName);
            }

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userToVerify.UserName, userToVerify.Id));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }

        public async Task<bool> EmailAlreadyTaken(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);

            return result != null;
        }
        
        public async Task<UploadImageResponse> UpdateProfileImage(Claim claim, IFormFile image)
        {
            var currentUser = await _userManager.FindByNameAsync(claim.Value);
            var fileName = Guid.NewGuid();
            var res = await _blobStorage.UploadBlob($"{currentUser.Id}/ProfilePics", image, fileName);
            if (string.IsNullOrEmpty(res.ToString()))
            {
                _logger.LogError("User " + currentUser.UserName + " Avi not updated");
                return new UploadImageResponse(false, new ImageErrors()
                {
                    IdentityErrors = null,
                    UploadError = "User " + currentUser.UserName + " avi could not be uploaded"
                });
            }

            currentUser.Avi = res.ToString();
            
            var result = await _userManager.UpdateAsync(currentUser);
            if (!result.Succeeded)
            {
                _logger.LogError($"Could not create user: {result.Errors}");
                return new UploadImageResponse(false, new ImageErrors()
                {
                    IdentityErrors = result.Errors,
                    UploadError = ""
                });
            }
            
            return new UploadImageResponse(true,null);

        }

        public async Task<int> ChangeLocationStatus(Claim claim, MarkLocationsRequest request)
        {
            var user = await _userManager.FindByNameAsync(claim.Value);

            // request.locations contains <locationName,Status>
            foreach (var i in request.Locations)
            {
                var location = _visitContext.Location.Single(f => f.LocationCode == i.Key);
                
                var userLocation = new UserLocation
                {
                    Status = i.Value,
                    Venue = "",
                    FkLocation = location,
                    FkUser = user
                };
                
                var existingEntry = _visitContext.UserLocation.SingleOrDefault(e =>
                    e.FkUser == user && e.FkLocation == location && e.Status == i.Value);

                if (existingEntry != null)
                {
                    existingEntry.Status = i.Value;
                    _visitContext.UserLocation.Update(existingEntry);
                    userLocation = existingEntry;
                }
                else
                {
                    _visitContext.UserLocation.Add(userLocation);
                }

                var caption = "";
                PostType postType;
                if (i.Value == "toVisit")
                {
                    caption = $"Wants to venture {location.LocationName}. Any thoughts?";
                    postType = _visitContext.PostType.SingleOrDefault(t => t.Type == "toVisit");
                }
                else
                {
                    caption = $"Has visited {location.LocationName}. Ask them about it!";
                    postType = _visitContext.PostType.SingleOrDefault(t => t.Type == "visited");
                }
                
                var post = new Post
                {
                    PostContentLink = $"",
                    FkPostType = postType,
                    PostCaption = caption,
                    PostTime = DateTime.UtcNow,
                    FkUser = user
                };
                _visitContext.Post.Add(post);

                _visitContext.PostUserLocation.Add(new PostUserLocation
                {
                    FkLocation = userLocation,
                    FkPost = post
                });
            }
            
            return  await _visitContext.SaveChangesAsync();
        }
        
        public async Task<CodeConfirmResult> ConfirmRegister(CodeConfirmRequest model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ChangePassword(ChangePasswordRequest model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ForgotPassword(ResetPasswordRequest model)
        {
            throw new NotImplementedException();
        }

        public async Task<CodeConfirmResult> ConfirmPasswordReset(SetNewPasswordWithCodeRequest model)
        {
            throw new NotImplementedException();
        }
    }
}