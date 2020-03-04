using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Amazon;
using Amazon.AspNetCore.Identity.Cognito.Exceptions;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Visit.Service.Config;

namespace Visit.Service.BusinessLogic
{
//    public class CognitoUserManager : IDisposable
//    {
//        private readonly IOptions<CognitoConfig> _config;
//        private string _token = "";
//        private readonly CognitoUserPool _pool;
//
//        public CognitoUserManager(IOptions<CognitoConfig> config, CognitoUserPool pool)
//        {
//            _config = config;
//            _pool = pool;
//        }
//
//        public AmazonCognitoIdentityProviderClient GetCognitoIdentityProvider()
//        {
//            RegionEndpoint region = _config.Value.RegionEndpoint;
//            AmazonCognitoIdentityProviderClient provider = null;
//
//            provider = new AmazonCognitoIdentityProviderClient(region);
//
//            return provider;
//        }
//
//        /// <summary>
//        ///    Logout registration by client
//        /// </summary>
//        public void Logout()
//        {
//            if (_user != null)
//            {
//                _user.SignOut();
//            }
//        }
//
//        private AuthenticationResultType Authentication(ApiLoginRequest _loginModel)
//        {
//            String userName = _loginModel.UserName;
//            String password = _loginModel.Password;
//
//            AuthenticationResultType status = null;
//
//            status = Authentication(userName, password);
//
//            return status;
//        }
//        
//        /// <summary>
//        ///    login and get JWT token
//        /// </summary>
//        /// <param name="userName"></param>
//        /// <param name="password"></param>
//        /// <returns>jwt token</returns>
//        public string Login(string userName, string password)
//        {
//            _token = null;
//            if (String.IsNullOrEmpty(userName))
//            {
//                throw new ArgumentNullException("userName");
//            }
//
//            if (String.IsNullOrEmpty(password))
//            {
//                throw new ArgumentNullException("password");
//            }
//
//            try
//            {
//                var authenticationResult = Authentication(userName, password);
//                if (authenticationResult != null)
//                {
//                    _token = authenticationResult.IdToken;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                _token = null;
//            }
//
//            return _token;
//        }
//
//        public ApiIdentityResult ChangePasswordWithToken(string username, string token, string newPassword)
//        {
//            ApiIdentityResult result = new ApiIdentityResult();
//            try
//            {
//                using (AmazonCognitoIdentityProviderClient userProvider = GetCognitoIdentityProvider())
//                {
//                    string poolID = _config.Value.UserPoolId;
//                    string clientID = _config.Value.UserPoolClientId;
//                    string clientSecret = _config.Value.UserPoolClientSecret;
//
//                    _pool = new CognitoUserPool(poolID,
//                        clientID, userProvider, clientSecret);
//
//                    var cancellationToken = new CancellationToken();
//                    _pool.ConfirmForgotPassword(username, token, newPassword, cancellationToken).ConfigureAwait(false)
//                        .GetAwaiter().GetResult();
//                    result.Succeeded = true;
//                    result.Token = token;
//                }
//            }
//            catch (Exception ex)
//            {
//                result.SetFailed(-200, ex.Message);
//                result.Succeeded = false;
//                result.Failed.Code = -200;
//                result.Failed.Description = ex.Message;
//            }
//
//            return result;
//        }
//
//
//        private bool UpdateAttributes(List<AttributeType> attributes)
//        {
//            try
//            {
//                var updates = attributes.ToDictionary(x => x.Name, x => x.Value);
//                _user.UpdateAttributesAsync(updates).GetAwaiter().GetResult();
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                return false;
//            }
//
//            return true;
//        }
//
//        public bool UpdateAttributes(ApiUpdateRequest updateInfo)
//        {
//            using (AmazonCognitoIdentityProviderClient userProvider = GetCognitoIdentityProvider())
//            {
//                var request = new AdminUpdateUserAttributesRequest();
//                request.Username = updateInfo.UserName;
//                request.UserPoolId = _config.Value.UserPoolId;
//                request.UserAttributes = updateInfo.Attributes.Select(data => new AttributeType()
//                {
//                    Name = data.Key,
//                    Value = data.Value
//                }).ToList();
//                userProvider.AdminUpdateUserAttributesAsync(request, CancellationToken.None).GetAwaiter().GetResult();
//            }
//
//            return true;
//        }
//
//        /// <summary>
//        ///    Reaset user password withowt preview password  
//        /// </summary>
//        /// <param name="requestObject"></param>
//        /// <returns></returns>
//        public bool AdminSetPassword(ApiAdminSetPasswordRequest requestObject)
//        {
//            using (AmazonCognitoIdentityProviderClient userProvider = GetCognitoIdentityProvider())
//            {
//                var request = new AdminSetUserPasswordRequest()
//                {
//                    Password = requestObject.Password,
//                    Username = requestObject.UserName,
//                    Permanent = false,
//                    UserPoolId = _config.Value.UserPoolId
//                };
//
//                userProvider.AdminSetUserPasswordAsync(request, CancellationToken.None).GetAwaiter().GetResult();
//            }
//
//            return true;
//        }
//
//        /// <summary>
//        ///    Change password by current user , with  username  and password 
//        /// </summary>
//        /// <param name="request"></param>
//        /// <returns></returns>
//        public bool ChangePassword(ApiChangePasswordRequest request)
//        {
//            using (var userProvider = GetCognitoIdentityProvider())
//            {
//                CognitoUser user = GetUser(request.UserName, userProvider);
//
//                InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest()
//                {
//                    Password = request.PreviewPassword
//                };
//
//                var authResponse = user.StartWithSrpAuthAsync(authRequest).GetAwaiter().GetResult();
//
//                if (authResponse.ChallengeName == ChallengeNameType.NEW_PASSWORD_REQUIRED)
//                {
//                    var result2 = user.RespondToNewPasswordRequiredAsync(new RespondToNewPasswordRequiredRequest()
//                    {
//                        SessionID = authResponse.SessionID,
//                        NewPassword = request.NewPassword
//                    }).ConfigureAwait(false).GetAwaiter().GetResult();
//                }
//
//                var task = user.ChangePasswordAsync(request.PreviewPassword, request.NewPassword).GetAwaiter();
//                task.GetResult();
//            }
//
//            return true;
//        }
//
//        public CognitoUser FindUser(string userName)
//        {
//            try
//            {
//                using (AmazonCognitoIdentityProviderClient userProvider = GetCognitoIdentityProvider())
//                {
//                    string poolID = _config.Value.UserPoolId;
//                    string clientID = _config.Value.UserPoolClientId;
//                    string clientSecret = _config.Value.UserPoolClientSecret;
//
//                    var pool = new CognitoUserPool(poolID,
//                        clientID, userProvider, clientSecret);
//
//
//                    var user = pool.FindByIdAsync(userName).ConfigureAwait(false).GetAwaiter().GetResult();
//
//
//                    return user as CognitoUser;
//                }
//            }
//            catch (AmazonCognitoIdentityProviderException e)
//            {
//                throw new CognitoServiceException("Failed to find the Cognito User by userName", e);
//            }
//        }
//
//        public bool DeleteUser(string userName)
//        {
//            using (AmazonCognitoIdentityProviderClient userProvider = GetCognitoIdentityProvider())
//            {
//                var result = userProvider.AdminDeleteUserAsync(new AdminDeleteUserRequest
//                {
//                    Username = userName,
//                    UserPoolId = _config.Value.UserPoolId
//                }, CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
//                return true;
//            }
//        }
//
//
//        /// <summary>
//        ///   Find users by properties 
//        /// </summary>
//        /// <example> 
//        ///    <code>
//        ///     https://docs.aws.amazon.com/cognito-user-identity-pools/latest/APIReference/API_ListUsers.html
//        ///    
//        ///     search for the following standard attributes:
//        ///     
//        /// You can only search for the following standard attributes:
//        ///        username(case-sensitive)
//        ///        email
//        ///        phone_number
//        ///        name
//        ///        given_name
//        ///        family_name
//        ///        preferred_username
//        ///       cognito:user_status(called Status in the Console) (case-insensitive)
//        ///       status(called Enabled in the Console) (case-sensitive)
//        ///       sub
//        ///
//        ///       Custom attributes are not searchable.
//        ///     </code>
//        /// </example>
//        /// <param name="attributesToGet"></param>
//        /// <param name="AttributeToFind"></param>
//        /// <param name="AttributeValue"></param>
//        /// <returns></returns>
//        public String Find(List<String> attributesToGet, string AttributeToFind, string AttributeValue)
//        {
//            String ret = "";
//            try
//            {
//                string poolID = _config.Value.UserPoolId;
//                RegionEndpoint region = _config.Value.RegionEndpoint;
//
//
//                ListUsersRequest request = new ListUsersRequest();
//                request.AttributesToGet = attributesToGet;
//                String Filter = $"{AttributeToFind} = \"" + AttributeValue + "\"";
//                request.Filter = Filter;
//                request.UserPoolId = poolID;
//                request.Limit = 50;
//
//
//                var credentional = _user.GetCognitoAWSCredentials(poolID, region);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                throw;
//            }
//
//            return ret;
//        }
//
//
//        /// <summary>
//        ///   Change password for current user
//        /// </summary>
//        /// <param name="PreviousPassword"></param>
//        /// <param name="newPassword"></param>
//        /// <returns></returns>
//        public Boolean ChangePassword(string PreviousPassword, string newPassword)
//        {
//            var user = _user;
//            if (user == null)
//            {
//                return false;
//            }
//
//            var task = user.ChangePasswordAsync(PreviousPassword, newPassword).GetAwaiter();
//            task.GetResult();
//            return true;
//        }
//
//        /// <summary>
//        ///   Check token by attribute.Cognito send SMS message
//        /// </summary>
//        /// <param name="token"></param>
//        /// <param name="attributeName"></param>
//        /// <param name="reconnect"></param>
//        /// <returns></returns>
//        public ApiIdentityResult VerifyToken(string token, String attributeName)
//        {
//            ApiIdentityResult ret = new ApiIdentityResult();
//            try
//            {
//                var request = new GetUserAttributeVerificationCodeRequest()
//                {
//                    AccessToken = token,
//                    AttributeName = attributeName
//                };
//
//                using (var provider = GetCognitoIdentityProvider())
//                {
//                    var responce = provider.GetUserAttributeVerificationCodeAsync(request).GetAwaiter().GetResult();
//                    if (responce.HttpStatusCode != System.Net.HttpStatusCode.OK)
//                    {
//                        ret.SetFailed(-100, responce.ResponseMetadata.ToString());
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                ret.SetFailed(-101, ex.Message);
//            }
//
//            return ret;
//        }
//
//
//        private CognitoUser _user { get; set; } = null;
//
//        private CognitoUser GetUser(String userName, String password, bool reconnect = false)
//        {
//            if (_user == null || reconnect)
//            {
//                Authentication(userName, password);
//            }
//
//            return _user;
//        }
//
//
//        public bool Register(ApiRegisterRequest input)
//        {
//            var ret = Register(input.UserName, input.Password, input.eMail);
//
//
//            if (ret)
//            {
//                if (input.Attributes != null && input.Attributes.Count > 0)
//                {
//                    Login(input.UserName, input.Password);
//                    var attrList = input.Attributes.Select(p => new AttributeType() {Name = p.Key, Value = p.Value})
//                        .ToList();
//
//                    ret = UpdateAttributes(attrList);
//                }
//            }
//
//
//            return ret;
//        }
//
//        public bool Register(string Username, string Password, String Email)
//        {
//            var credentional = new AnonymousAWSCredentials();
//
//            bool response = false;
//            using (AmazonCognitoIdentityProviderClient cognito = GetCognitoIdentityProvider())
//            {
//                try
//                {
//                    var controller = new CognitoSignUpController(cognito);
//
//                    response = controller.SignUpAsync(Username, Password, Email).GetAwaiter().GetResult();
//
//                    if (response)
//                    {
//                        var signUpConfirmRequest = new AdminConfirmSignUpRequest();
//                        signUpConfirmRequest.Username = Username;
//                        signUpConfirmRequest.UserPoolId = _config.Value.UserPoolId;
//
//                        var ret = cognito.AdminConfirmSignUpAsync(signUpConfirmRequest, CancellationToken.None)
//                            .GetAwaiter().GetResult();
//                    }
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine(ex.Message);
//                    throw;
//                }
//
//                return response;
//            }
//        }
//
//
//        public AuthenticationResultType GetAuthentication(string userName, string password)
//        {
//            using (AmazonCognitoIdentityProviderClient userProvider = GetCognitoIdentityProvider())
//            {
//                if (String.IsNullOrEmpty(userName))
//                {
//                    throw new ArgumentNullException("userName");
//                }
//
//                if (String.IsNullOrEmpty(password))
//                {
//                    throw new ArgumentNullException("password");
//                }
//
//                AuthenticationResultType status = null;
//
//                _user = GetUser(userName, userProvider);
//
//                InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest()
//                {
//                    Password = password
//                };
//
//                try
//                {
//                    var authResponse = _user.StartWithSrpAuthAsync(authRequest).GetAwaiter().GetResult();
//
//                    if (authResponse != null)
//                    {
//                        status = authResponse.AuthenticationResult;
//
//
//                        if (authResponse.ChallengeName == ChallengeNameType.NEW_PASSWORD_REQUIRED)
//                        {
//                            var result2 = _user.RespondToNewPasswordRequiredAsync(
//                                new RespondToNewPasswordRequiredRequest()
//                                {
//                                    SessionID = authResponse.SessionID,
//                                    NewPassword = password
//                                }).ConfigureAwait(false).GetAwaiter().GetResult();
//
//                            status = result2.AuthenticationResult;
//                        }
//
//
//                        String idToken = status.IdToken,
//                            accessToken = status.AccessToken,
//                            refreshToken = status.RefreshToken;
//
//
//                        var user2 = GetUser(userName, userProvider);
//
//                        user2.SessionTokens = new CognitoUserSession(idToken, accessToken, refreshToken, DateTime.Now,
//                            DateTime.Now.AddDays(10));
//
//
//                        var result = user2.StartWithRefreshTokenAuthAsync(
//                            new InitiateRefreshTokenAuthRequest()
//                            {
//                                AuthFlowType = AuthFlowType.REFRESH_TOKEN
//                            }
//                        ).ConfigureAwait(false).GetAwaiter().GetResult();
//                    }
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine(ex.Message);
//                    status = null;
//                }
//
//                if (status == null)
//                {
//                    _user = null;
//                }
//                else
//                {
//                    if (status != null)
//                    {
//                        _token = status.IdToken;
//                    }
//                }
//
//                return status;
//            }
//        }
//
//
//        private AuthenticationResultType Authentication(string userName, string password)
//        {
//            AmazonCognitoIdentityProviderClient userProvider = GetCognitoIdentityProvider();
//            {
//                AuthenticationResultType status = null;
//
//                _user = GetUser(userName, userProvider);
//
//                InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest()
//                {
//                    Password = password
//                };
//
//                try
//                {
//                    var authResponse = _user.StartWithSrpAuthAsync(authRequest).GetAwaiter().GetResult();
//
//                    if (authResponse != null)
//                    {
//                        status = authResponse.AuthenticationResult;
//
//
//                        if (authResponse.ChallengeName == ChallengeNameType.NEW_PASSWORD_REQUIRED)
//                        {
//                            var result2 = _user.RespondToNewPasswordRequiredAsync(
//                                new RespondToNewPasswordRequiredRequest()
//                                {
//                                    SessionID = authResponse.SessionID,
//                                    NewPassword = password
//                                }).ConfigureAwait(false).GetAwaiter().GetResult();
//
//                            status = result2.AuthenticationResult;
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine(ex.Message);
//                    status = null;
//                }
//
//                if (status == null)
//                {
//                    _user = null;
//                }
//                else
//                {
//                    if (status != null)
//                    {
//                        _token = status.IdToken;
//                    }
//                }
//
//                return status;
//            }
//        }
//
//
//        private void AddUserTokensToAuthenticationProperties(CognitoUser user,
//            AuthenticationProperties authenticationProperties)
//        {
//            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
//            {
//                new AuthenticationToken()
//                {
//                    Name = OpenIdConnectParameterNames.AccessToken,
//                    Value = user.SessionTokens?.AccessToken
//                },
//                new AuthenticationToken()
//                {
//                    Name = OpenIdConnectParameterNames.RefreshToken,
//                    Value = user.SessionTokens?.RefreshToken
//                },
//                new AuthenticationToken()
//                {
//                    Name = OpenIdConnectParameterNames.IdToken,
//                    Value = user.SessionTokens?.IdToken
//                }
//            });
//        }
//        
//        public List<ApiUserInfo> GetUserList()
//        {
//            var result = new List<ApiUserInfo>();
//            using (var provider = (new CognitoUserManager()).GetCognitoIdentityProvider())
//            {
//                var request = new ListUsersRequest
//                {
//                    UserPoolId = _config.Value.UserPoolId,
//                    Filter = ""
//                };
//
//
//                ListUsersResponse response = null;
//
//
//                do
//                {
//                    request.PaginationToken = response?.PaginationToken;
//                    try
//                    {
//                        response = provider.ListUsersAsync(request, System.Threading.CancellationToken.None)
//                            .ConfigureAwait(false).GetAwaiter().GetResult();
//                    }
//                    catch (AmazonCognitoIdentityProviderException e)
//                    {
//                        throw new CognitoServiceException("Failed to retrieve the list of users from Cognito.", e);
//                    }
//
//                    foreach (var user in response.Users)
//                    {
//                        var info = new ApiUserInfo();
//                        info.Name = user.Username;
//                        info.Status = user.UserStatus.Value;
//                        info.Attributes =
//                            user.Attributes.ToDictionary(attribute => attribute.Name, attribute => attribute.Value);
//                        result.Add(info);
//                    }
//                } while (!string.IsNullOrEmpty(response.PaginationToken));
//            }
//
//            return result;
//        }
//
//        private CognitoUser GetUser(string userName, AmazonCognitoIdentityProviderClient userProvider)
//        {
//            {
//                CognitoUser user = new CognitoUser(userName, _config.Value.UserPoolClientId, _pool,
//                    userProvider, _config.Value.UserPoolClientSecret,
//                    username: userName);
//
//                return user;
//            }
//        }
//
//        #region IDisposable Support
//
//        private bool disposedValue = false; // To detect redundant calls
//
//        protected virtual void Dispose(bool disposing)
//        {
//            if (!disposedValue)
//            {
//                if (disposing)
//                {
//                    if (_user != null)
//                    {
//                        _user = null;
//                    }
//                }
//
//                disposedValue = true;
//            }
//        }
//
//        ~CognitoUserManager()
//        {
//            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
//            Dispose(false);
//        }
//
//
//        void IDisposable.Dispose()
//        {
//            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
//            Dispose(true);
//            // GC.SuppressFinalize(this);
//        }
//
//        #endregion
//    }
}