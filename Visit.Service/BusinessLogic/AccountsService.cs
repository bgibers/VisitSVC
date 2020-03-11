using System;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using Visit.DataAccess.Auth;
using Visit.DataAccess.Auth.Helpers;
using Visit.DataAccess.EntityFramework;
using Visit.DataAccess.Models;
using Visit.Service.BusinessLogic.BlobStorage;
using Visit.Service.BusinessLogic.Interfaces;
using Visit.Service.Models;
using Visit.Service.Models.Enums;

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
            JwtIssuerOptions jwtOptions, VisitContext visitContext, 
            UserManager<User> userManager, IBlobStorageBusinessLogic blobStorage)
        {
            _logger = logger;
            _mapper = mapper;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions;
            _visitContext = visitContext;
            _userManager = userManager;
            _blobStorage = blobStorage;
        }
        
        public async Task<UserApi> RegisterUser(RegisterModelApi model)
        {
            var userIdentity = _mapper.Map<User>(model);

            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            // todo add image upload to controller and service
            
            if (!result.Succeeded)
            {
                _logger.LogError($"Could not create user: {result.Errors}");
                return null;
            }

            await _visitContext.SaveChangesAsync();

            return _mapper.Map<UserApi>(userIdentity);
        }
        
        public async Task<JwtToken> LoginUser(LoginApiModel credentials)
        {
            var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);
            
            if (identity == null)
            {
                return null;
            }
            
            var token = JsonConvert.DeserializeObject<JwtToken>(await Tokens.GenerateJwt(identity, _jwtFactory, identity.Name, _jwtOptions, 
                new JsonSerializerSettings { Formatting = Formatting.Indented }));
            
            return token;
        }
        
        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);
            
            User userToVerify;
            if (userName.Contains('@'))
            {
                //need to fix this
                IsEmailValid(userName);
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

        public async Task<bool> UserNameEmailTaken(string login)
        {
            if (login.Contains('@'))
            {
                //need to fix this
                IsEmailValid(login);
                var result = await _userManager.FindByEmailAsync(login);

                if (result != null) return true;
            }
            else
            {
                var result = await _userManager.FindByNameAsync(login);
                if (result != null) return true;
            }

            return false;
        }

        private bool IsEmailValid(string mail)
        {
            try
            {                
                MailAddress eMailAddress = new MailAddress(mail);
                return true;
            }
            catch (FormatException)
            {
                return false;  
            }
        }
        
        public async Task<IdentityResult> UploadProfileImage(IFormFile image, Claim user)
        {
            var currentUser = await _userManager.FindByNameAsync(user.Value);

            if (!await _blobStorage.UploadFile(currentUser.Id, image))
            {
                throw new StorageException("User " + currentUser.UserName + " Avi not updated");
            }

            // todo change this to be the url of the avi
            currentUser.Avi = currentUser.Id;
            return await _userManager.UpdateAsync(currentUser);
        }
        
        public async Task<CodeConfirmResult> ConfirmRegister(CodeConfirmApi model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ChangePassword(ChangePasswordApi model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ForgotPassword(ResetPasswordRequestApi model)
        {
            throw new NotImplementedException();
        }

        public async Task<CodeConfirmResult> ConfirmPasswordReset(ResetPasswordApi model)
        {
            throw new NotImplementedException();
        }
    }
}