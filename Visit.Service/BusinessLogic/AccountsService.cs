using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly VisitContext _visitContext;
        private readonly IBlobStorageBusinessLogic _blobStorage;
        private readonly IFirebaseService _firebaseService;
        
        public AccountsService(ILogger<AccountsService> logger, IMapper mapper, VisitContext visitContext, 
            IBlobStorageBusinessLogic blobStorage, IFirebaseService firebaseService)
        {
            _logger = logger;
            _mapper = mapper;
            _visitContext = visitContext;
            _blobStorage = blobStorage;
            _firebaseService = firebaseService;
        }
        
        public async Task<bool> RegisterUser(RegisterRequest model)
        {
            var userIdentity = _mapper.Map<User>(model);
            var result = await _firebaseService.CreateUser(model.Email, model.Password);
            userIdentity.Id = result.Uid;

            await _visitContext.User.AddAsync(userIdentity);
            await _visitContext.SaveChangesAsync();
                
            return true;
        }
        
        public async Task<bool> EmailAlreadyTaken(string email)
        {
//            var message = new Message()
//            {
//                Token = "e8AMdTGkKksiqvpwVDNivC:APA91bGQYcDwblCzacTwNTzjXy8v8WCb6GuH1VW3WGGwIsVfxbEp-BuFhKTLBgpXx434E7NglH_RFUCok_BNt0TfTEGtZ5xIPEvx3BjURWMEE05zICJO8NKbes2E1c0b11tGEMh7SM75",
//                Notification = new Notification()
//                {
//                    Body = "Test",
//                    Title = "test"
//                }
//            };
//
//            await _firebaseService.SendPushNotification(message);
//
//            
            var result = await _firebaseService.GetUserByEmail(email);

            return result != null;
        }

        public async Task<UploadImageResponse> UpdateProfileImage(string claim, IFormFile image)
        {
            var userId = (await _firebaseService.GetUserFromToken(claim)).Uid;
            var currentUser = await _visitContext.User.FindAsync(userId);
            
            var fileName = Guid.NewGuid();
            var res = await _blobStorage.UploadBlob($"{currentUser.Id}/ProfilePics", image, fileName);
            if (string.IsNullOrEmpty(res.ToString()))
            {
                _logger.LogError("User " + currentUser.Email + " Avi not updated");
                return new UploadImageResponse(false, new ImageErrors()
                {
                    IdentityErrors = null,
                    UploadError = "User " + currentUser.Email + " avi could not be uploaded"
                });
            }

            currentUser.Avi = res.ToString();
            
            _visitContext.User.Update(currentUser);
            await _visitContext.SaveChangesAsync();
            
            return new UploadImageResponse(true,null);

        }

        public async Task<bool> UpdateAccountInfo(string claim, UpdateUserInfoRequest request)
        {
            var userId = (await _firebaseService.GetUserFromToken(claim)).Uid;
            var user = await _visitContext.User.FindAsync(userId);
            
            user.Education = request.Education;
            user.Firstname = request.Firstname;
            user.Lastname = request.Lastname;
            user.BirthLocation = request.BirthLocation;
            user.ResidenceLocation = request.ResidenceLocation;
            user.Title = request.Title;

            _visitContext.User.Update(user);
            await _visitContext.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateUserFcm(string claim, string deviceId)
        {
            var userId = (await _firebaseService.GetUserFromToken(claim)).Uid;
            var user = await _visitContext.User.FindAsync(userId);
            
            _visitContext.User.Update(user);
            await _visitContext.SaveChangesAsync();
            
            return true;
        }

        public async Task<int> ChangeLocationStatus(string claim, MarkLocationsRequest request)
        {
            var userId = (await _firebaseService.GetUserFromToken(claim)).Uid;
            var user = await _visitContext.User.FindAsync(userId);
            
            // request.locations contains <locationName,Status>
            foreach (var (key, value) in request.Locations)
            {
                var location = _visitContext.Location.Single(f => f.LocationCode == key);
                
                var userLocation = new UserLocation
                {
                    Status = value,
                    Venue = "",
                    FkLocation = location,
                    FkUser = user
                };
                
                var existingEntry = _visitContext.UserLocation.SingleOrDefault(e =>
                    e.FkUser == user && e.FkLocation == location && e.Status == value);

                if (existingEntry != null)
                {
                    existingEntry.Status = value;
                    _visitContext.UserLocation.Update(existingEntry);
                    userLocation = existingEntry;
                }
                else
                {
                    _visitContext.UserLocation.Add(userLocation);
                }

                var caption = "";
                PostType postType;
                if (value == "toVisit")
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