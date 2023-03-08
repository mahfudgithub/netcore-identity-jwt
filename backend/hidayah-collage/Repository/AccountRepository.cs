using hidayah_collage.DataContext;
using hidayah_collage.Interface;
using hidayah_collage.Models;
using hidayah_collage.Models.Email;
using hidayah_collage.Models.TokenGenerator;
using hidayah_collage.Models.TokenValidator;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hidayah_collage.Repository
{
    public class AccountRepository : IAccount
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;
        private readonly GetMessageRepository _getMessageRepository;
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly IRefreshToken _refreshToken;
        private readonly RefreshTokenValidator _refreshTokenValidator;
        private readonly SystemMasterRepository _systemMasterRepository;

        public AccountRepository(UserManager<ApplicationUser> userManager
            ,SignInManager<ApplicationUser> signInManager
            ,IConfiguration configuration
            ,IMailService mailService
            ,GetMessageRepository getMessageRepository
            ,AccessTokenGenerator accessTokenGenerator
            ,RefreshTokenGenerator refreshTokenGenerator
            ,IRefreshToken refreshToken
            ,RefreshTokenValidator refreshTokenValidator
            ,SystemMasterRepository systemMasterRepository
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _mailService = mailService;
            _getMessageRepository = getMessageRepository;
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshToken = refreshToken;
            _refreshTokenValidator = refreshTokenValidator;
            _systemMasterRepository = systemMasterRepository;
        }

        public async Task<WebResponse> Login(LoginRequest loginRequest)
        {
            WebResponse webResponse = new WebResponse();
            var response = new LoginResponse();
            ApplicationUser applicationUser = new ApplicationUser();
            //var email = new EmailAddressAttribute();
            try
            {
                var userName = loginRequest.Email;
                if (userName.IndexOf('@') > -1)
                {
                    applicationUser = await ValidateEmail(loginRequest.Email);
                    if (applicationUser == null)
                    {
                        webResponse.status = false;
                        webResponse.message = _getMessageRepository.GetMeessageText("ERR002");
                        webResponse.data = null;
                        return webResponse;
                    }
                }
                else
                {
                    applicationUser = await ValidateUserName(loginRequest.Email);
                    if (applicationUser == null)
                    {
                        webResponse.status = false;
                        webResponse.message = _getMessageRepository.GetMeessageText("ERR002");
                        webResponse.data = null;
                        return webResponse;
                    }
                }

                //if (email.IsValid(loginRequest.Email))
                //{
                //    applicationUser = await ValidateEmail(loginRequest.Email);
                //    if (applicationUser == null)
                //    {
                //        webResponse.status = false;
                //        webResponse.message = _getMessageRepository.GetMeessageText("ERR002");
                //        webResponse.data = null;
                //        return webResponse;
                //    }
                //}
                //else
                //{
                //    applicationUser = await ValidateUserName(loginRequest.Email);
                //    if (applicationUser == null)
                //    {
                //        webResponse.status = false;
                //        webResponse.message = _getMessageRepository.GetMeessageText("ERR002");
                //        webResponse.data = null;
                //        return webResponse;
                //    }
                //}

                var result = await _signInManager.PasswordSignInAsync(applicationUser.UserName, loginRequest.Password, false, false);

                if (!result.Succeeded)
                {
                    webResponse.status = false;
                    webResponse.message = _getMessageRepository.GetMeessageText("ERR003");
                    webResponse.data = null;
                    return webResponse;
                }

                var generateToken = _accessTokenGenerator.GenerateToken(applicationUser);
                var refreshToken = _refreshTokenGenerator.GenerateToken();

                RefreshToken refreshTokenDTO = new RefreshToken()
                {
                    Token = refreshToken,
                    UserId = applicationUser.Id
                };
                await _refreshToken.Create(refreshTokenDTO);

                //response.FirstName = applicationUser.FirstName;
                //response.LastName = applicationUser.LastName;
                //response.Username = applicationUser.Email;
                response.Token = generateToken.Token;
                response.ExpireDate = generateToken.ExpireDate;
                response.RefreshToken = refreshToken;

                webResponse.status = true;
                webResponse.message = _getMessageRepository.GetMeessageText("SUC003");
                webResponse.data = response;
                /*
                var authClaims = new List<Claim>
                {
                    //new Claim("id", emailAccount.Result.Id),
                    new Claim("Email", emailAccount.Result.Email),
                    new Claim(ClaimTypes.NameIdentifier, emailAccount.Result.Id)
                    //new Claim(ClaimTypes.Name, loginRequest.Email),
                    //new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };

                var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddDays(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256)
                    );

                string tokenAsSting = new JwtSecurityTokenHandler().WriteToken(token);
                if (tokenAsSting != null)
                {
                    response.FirstName = emailAccount.Result.FirstName;
                    response.LastName = emailAccount.Result.LastName;
                    response.Username = emailAccount.Result.Email;
                    response.Token = tokenAsSting;
                    response.ExpireDate = token.ValidTo;

                    webResponse.status = true;
                    webResponse.message = _getMessageRepository.GetMeessageText("SUC003");
                    webResponse.data = response;
                }
                */
            }
            catch (Exception ex)
            {
                webResponse.status = false;
                webResponse.message = $"Exception occurred with a message: {ex.Message}";
                webResponse.data = null;
            }
            return webResponse;
        }

        public async Task<WebResponse> Register(RegisterRequest registerRequest)
        {
            WebResponse webResponse = new WebResponse();
            var response = new RegisterResponse();
            EmailRequest emailRequest = new EmailRequest();

            var user = new ApplicationUser()
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Email = registerRequest.Email,
                UserName = registerRequest.UserName
            };

            if(registerRequest.Password != registerRequest.ConfirmPassword)
            {
                webResponse.status = false;
                webResponse.message = _getMessageRepository.GetMeessageText("ERR004");
                webResponse.data = null;
                return webResponse;
            }

            try
            {
                var emailAccount = await ValidateEmail(registerRequest.Email);
                if (emailAccount != null)
                {
                    var messageTxt = _getMessageRepository.GetMeessageText("ERR001");
                    webResponse.status = false;
                    webResponse.message = messageTxt;
                    webResponse.data = null;
                    return webResponse;
                }

                var usernameAccount = await ValidateUserName(registerRequest.UserName);
                if (usernameAccount != null)
                {
                    var messageTxt = _getMessageRepository.GetMeessageText("ERR001");
                    webResponse.status = false;
                    webResponse.message = messageTxt;
                    webResponse.data = null;
                    return webResponse;
                }

                var result = await _userManager.CreateAsync(user, registerRequest.Password);

                if (result.Succeeded)
                {
                    /*
                    var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
                    var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

                    var Url = $"{_configuration["AppUrl"]}/api/Account/ConfirmEmail?userId={ user.Id}&token={ validEmailToken}";

                    string subject = "Confirm Your Email";
                    string body = "Please confirm your email by clicking <a href=\"" + Url + "\">Clicking here</a>";

                    emailRequest.Subject = subject;
                    emailRequest.ToEmail = user.Email;
                    emailRequest.Body = body;
                    //var mail = _mailService.SendEmailAsync(emailRequest);
                    //await _mailService.SendEmailSMTPAsync(emailRequest);
                    */
                    response.FirstName = registerRequest.FirstName;
                    response.Username = registerRequest.Email;
                    response.Email = registerRequest.Email;
                    webResponse.status = true;
                    webResponse.message = _getMessageRepository.GetMeessageText("SUC001");
                    webResponse.data = response;
                }
                else
                {
                    webResponse.status = false;
                    webResponse.message = String.Join(", ", result.Errors.Select(x => x.Description));
                    webResponse.data = null;
                }
            }
            catch(Exception ex)
            {
                webResponse.status = false;
                webResponse.message = $"Exception occurred with a message: {ex.Message}";
                webResponse.data = null;
            }

            return webResponse;
        }

        public async Task<ApplicationUser> ValidateEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user;
        }

        public async Task<ApplicationUser> ValidateUserName(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            return user;
        }

        public async Task<WebResponse> ForgotPassword(ForgotPasswordRequest forgotPasswordRequest)
        {
            WebResponse webResponse = new WebResponse();
            var response = new ForgotPasswordResponse();
            EmailRequest emailRequest = new EmailRequest();

            try
            {
                var user = await ValidateEmail(forgotPasswordRequest.Email);
                if (user == null)
                {
                    webResponse.status = false;
                    webResponse.message = _getMessageRepository.GetMeessageText("ERR002");
                    webResponse.data = null;
                    return webResponse;
                }

            
                var systemMaster = await _systemMasterRepository.GetListMasterByType("RESET_PWD");
                if (systemMaster == null)
                {
                    webResponse.status = false;
                    webResponse.message = "System Master Not Found";
                    webResponse.data = null;
                    return webResponse;
                }
                var url = systemMaster.Where(x => x.Code == "URL").FirstOrDefault().Value_Txt;
                var content = systemMaster.Where(x => x.Code == "BODY").FirstOrDefault().Value_Txt;
                var link = systemMaster.Where(x => x.Code == "LINK").FirstOrDefault().Value_Txt;
                var subject = systemMaster.Where(x => x.Code == "SUBJECT").FirstOrDefault().Value_Txt;

                if (url == null || content == null || link == null || subject == null)
                {
                    webResponse.status = false;
                    webResponse.message = "System Master Not Complete";
                    webResponse.data = null;
                    return webResponse;
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var encodedToken = Encoding.UTF8.GetBytes(code);
                var validToken = WebEncoders.Base64UrlEncode(encodedToken);

                url = url.Replace("{url}", _configuration["AppClientUrl"]);
                url = url.Replace("{0}", user.Email);
                url = url.Replace("{1}", validToken);
                //var Url = $"{_configuration["AppUrl"]}/api/Account/ConfirmEmail?userId={ user.Id }&token={ validEmailToken }";



                //string content = "<html><head> <style> body, html, table {font-family: Nunito Sans, Helvetica Neue, Helvetiva, Arial, sans-serif;} table { border:0 } tbody td {text-align: center; height:35; width:160; background-color:#200e32;} a {text-decoration:none; color:white; display:inline-block; line-height:35px;width:150;}</style>   </head><body>Welcome to Collage School !<br>  <br>Please Confirm your email address. <br> <br> <table><tbody><tr><td>{url}</td></tr></tbody></table> <br> <br> Thank you.<br></body></html>";

                link = link.Replace("{url}", url);
                //string link = "<a target=\"_blank\" href=\"" + newUrl + "\">Confirm Email</a>";

                //string subject = "Confirm Your Email - Collage School";
                //string body = "Please confirm your email by clicking <a href=\"" + Url + "\">Confirm</a>";
                string body = content.Replace("{url}", link);
                /*
                //var callbackUrl = new Uri($"{_configuration["AppClientUrl"]}"+@"/ResetPassword?email=" + user.Result.Email + "&token=" + validToken);
                var callbackUrl = new Uri($"{_configuration["AppClientUrl"]}" + @"/account/resetpassword?email=" + user.Result.Email + "&token=" + validToken);
                //string subject = "Reset Password";
                string targetBody = "_blank";
                string rel = "noopener noreferrer";
                string body = "Please reset your password by clicking <a target=\"" + targetBody + "\" rel=\""+rel+"\" href=\"" + callbackUrl + "\"> Clicking here</a>";
                */
                emailRequest.Subject = subject;
                emailRequest.ToEmail = user.Email;
                emailRequest.Body = body;
                //var mail = _mailService.SendEmailAsync(emailRequest);
                await _mailService.SendEmailSMTPAsync(emailRequest);

                response.status = true;
                response.body = body;
                webResponse.status = true;
                webResponse.message = _getMessageRepository.GetMeessageText("SUC002");
                webResponse.data = response;
            }
            catch (Exception ex)
            {
                webResponse.status = false;
                webResponse.message = $"Exception occurred with a message: {ex.Message}";
                webResponse.data = null;
            }

            return webResponse;
        }

        public async Task<WebResponse> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            WebResponse webResponse = new WebResponse();
            var response = new ResetPasswordResponse();

            if (resetPasswordRequest.NewPassword != resetPasswordRequest.ConfirmNewPassword)
            {
                webResponse.status = false;
                webResponse.message = _getMessageRepository.GetMeessageText("ERR004");
                webResponse.data = null;
                return webResponse;
            }

            try
            {

                var user = await _userManager.FindByEmailAsync(resetPasswordRequest.Email);

                if (user != null)
                {
                    var decodedToken = WebEncoders.Base64UrlDecode(resetPasswordRequest.Token);
                    var normalToken = Encoding.UTF8.GetString(decodedToken);

                    var result = await _userManager.ResetPasswordAsync(user, normalToken, resetPasswordRequest.NewPassword);

                    if (result.Succeeded)
                    {
                        response.Email = user.Email;
                        webResponse.status = true;
                        webResponse.message = _getMessageRepository.GetMeessageText("SUC007");
                        webResponse.data = response;
                    }
                    else
                    {
                        webResponse.status = false;
                        webResponse.message = String.Join(", ", result.Errors.Select(x => x.Description));
                        webResponse.data = null;
                    }
                }
                else
                {
                    var messageTxt = _getMessageRepository.GetMeessageText("ERR002");
                    webResponse.status = false;
                    webResponse.message = messageTxt;
                    webResponse.data = null;
                }
            }
            catch(Exception ex)
            {
                webResponse.status = false;
                webResponse.message = $"Exception occurred with a message: {ex.Message}";
                webResponse.data = null;
            }

            return webResponse;

        }

        public async Task<WebResponse> ConfirmEmailAsync(string userId, string token)
        {
            WebResponse webResponse = new WebResponse();
            //var response = new ResetPasswordResponse();
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user != null)
                {
                    var decodedToken = WebEncoders.Base64UrlDecode(token);
                    var normalToken = Encoding.UTF8.GetString(decodedToken);

                    var result = await _userManager.ConfirmEmailAsync(user, normalToken);

                    if (result.Succeeded)
                    {
                        webResponse.status = true;
                        webResponse.message = _getMessageRepository.GetMeessageText("SUC001");
                    }
                    else
                    {
                        webResponse.status = false;
                        webResponse.message = String.Join(", ", result.Errors.Select(x => x.Description));
                        webResponse.data = null;
                    }
                }
                else
                {
                    webResponse.status = false;
                    webResponse.message = _getMessageRepository.GetMeessageText("ERR002");
                    webResponse.data = null;
                }
            }
            catch(Exception ex)
            {
                webResponse.status = false;
                webResponse.message = $"Exception occurred with a message: {ex.Message}";
                webResponse.data = null;
            }

            return webResponse;
        }

        public async Task<WebResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            WebResponse webResponse = new WebResponse();
            var response = new LoginResponse();
            RefreshToken refreshTokenDTO = new RefreshToken();

            try
            {
                bool isValidRefreshToken = _refreshTokenValidator.Validate(refreshTokenRequest.RefreshToken);
                if (!isValidRefreshToken)
                {
                    webResponse.status = false;
                    webResponse.message = _getMessageRepository.GetMeessageText("ERR005");
                    webResponse.data = null;
                    return webResponse;
                }

                refreshTokenDTO = await _refreshToken.GetByToken(refreshTokenRequest.RefreshToken);
                if (refreshTokenDTO == null)
                {
                    webResponse.status = false;
                    webResponse.message = _getMessageRepository.GetMeessageText("ERR005");
                    webResponse.data = null;
                    return webResponse;
                }

                await _refreshToken.Delete(refreshTokenDTO.Id);

                var user = await _userManager.FindByIdAsync(refreshTokenDTO.UserId);
                if (user == null)
                {
                    webResponse.status = false;
                    webResponse.message = _getMessageRepository.GetMeessageText("ERR002");
                    webResponse.data = null;
                    return webResponse;
                }

                var generateToken = _accessTokenGenerator.GenerateToken(user);
                var refreshToken = _refreshTokenGenerator.GenerateToken();

                refreshTokenDTO.Token = refreshToken;
                refreshTokenDTO.UserId = user.Id;

                await _refreshToken.Create(refreshTokenDTO);

                //response.FirstName = user.FirstName;
                //response.LastName = user.LastName;
                //response.Username = user.Email;
                response.Token = generateToken.Token;
                response.ExpireDate = generateToken.ExpireDate;
                response.RefreshToken = refreshToken;

                webResponse.status = true;
                webResponse.message = _getMessageRepository.GetMeessageText("SUC005");
                webResponse.data = response;
            }
            catch(Exception ex)
            {
                webResponse.status = false;
                webResponse.message = $"Exception occurred with a message: {ex.Message}";
                webResponse.data = null;
            }

            return webResponse;
        }

        public async Task<WebResponse> Logout(string rawUserId)
        {
            WebResponse webResponse = new WebResponse();
            if (!Guid.TryParse(rawUserId, out Guid userId))
            {
                webResponse.status = false;
                webResponse.message = _getMessageRepository.GetMeessageText("ERR006");
                webResponse.data = null;
                return webResponse;
            }

            await _refreshToken.DeleteAll(rawUserId);

            webResponse.status = true;
            webResponse.message = _getMessageRepository.GetMeessageText("SUC004");
            webResponse.data = null;
            return webResponse;
        }

        public async Task<WebResponse> GetUserInfo(string userId)
        {
            WebResponse webResponse = new WebResponse();

            try
            {
                var result = await _userManager.FindByIdAsync(userId);
                if (result == null)
                {
                    webResponse.status = false;
                    webResponse.message = _getMessageRepository.GetMeessageText("ERR002");
                    webResponse.data = null;
                    return webResponse;
                }
                var user = new ApplicationUser()
                {
                    FirstName = result.FirstName,
                    LastName = result.LastName,
                    Email = result.Email,
                    UserName = result.UserName,
                    Id = result.Id,
                    EmailConfirmed = result.EmailConfirmed,
                    PhoneNumber = result.PhoneNumber,
                    ConcurrencyStamp = null,
                    SecurityStamp = null
                };

                webResponse.status = true;
                webResponse.message = _getMessageRepository.GetMeessageText("SUC012");
                webResponse.data = user;
            }
            catch(Exception ex)
            {
                webResponse.status = false;
                webResponse.message = $"Exception occurred with a message: {ex.Message}";
                webResponse.data = null;
            }

            return webResponse;
        }

        public async Task<WebResponse> SendConfirmedEmail(string userId)
        {
            WebResponse webResponse = new WebResponse();
            EmailRequest emailRequest = new EmailRequest();

            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    webResponse.status = false;
                    webResponse.message = _getMessageRepository.GetMeessageText("ERR002");
                    webResponse.data = null;
                    return webResponse;
                }

                var systemMaster = await _systemMasterRepository.GetListMasterByType("EMAIL_CONFIRM");
                if (systemMaster == null)
                {
                    webResponse.status = false;
                    webResponse.message = "System Master Not Found";
                    webResponse.data = null;
                    return webResponse;
                }
                var newUrl = systemMaster.Where(x => x.Code == "URL").FirstOrDefault().Value_Txt;
                var newContent = systemMaster.Where(x => x.Code == "BODY").FirstOrDefault().Value_Txt;
                var newLink = systemMaster.Where(x => x.Code == "LINK").FirstOrDefault().Value_Txt;
                var newSubject = systemMaster.Where(x => x.Code == "SUBJECT").FirstOrDefault().Value_Txt;

                if (newUrl == null || newContent == null || newLink == null || newSubject == null)
                {
                    webResponse.status = false;
                    webResponse.message = "System Master Not Complete";
                    webResponse.data = null;
                    return webResponse;
                }

                var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
                var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);


                newUrl = newUrl.Replace("{url}", _configuration["AppUrl"]);
                newUrl = newUrl.Replace("{0}", userId);
                newUrl = newUrl.Replace("{1}", validEmailToken);
                //var Url = $"{_configuration["AppUrl"]}/api/Account/ConfirmEmail?userId={ user.Id }&token={ validEmailToken }";



                //string content = "<html><head> <style> body, html, table {font-family: Nunito Sans, Helvetica Neue, Helvetiva, Arial, sans-serif;} table { border:0 } tbody td {text-align: center; height:35; width:160; background-color:#200e32;} a {text-decoration:none; color:white; display:inline-block; line-height:35px;width:150;}</style>   </head><body>Welcome to Collage School !<br>  <br>Please Confirm your email address. <br> <br> <table><tbody><tr><td>{url}</td></tr></tbody></table> <br> <br> Thank you.<br></body></html>";

                newLink = newLink.Replace("{newurl}", newUrl);
                //string link = "<a target=\"_blank\" href=\"" + newUrl + "\">Confirm Email</a>";

                //string subject = "Confirm Your Email - Collage School";
                //string body = "Please confirm your email by clicking <a href=\"" + Url + "\">Confirm</a>";
                string body = newContent.Replace("{url}", newLink);

                emailRequest.Subject = newSubject;
                emailRequest.ToEmail = user.Email;
                emailRequest.Body = body;

                await _mailService.SendEmailSMTPAsync(emailRequest);

                webResponse.status = true;
                webResponse.message = _getMessageRepository.GetMeessageText("SUC013");
                webResponse.data = body;
            }
            catch(Exception ex)
            {
                webResponse.status = false;
                webResponse.message = $"Exception occurred with a message: {ex.Message}";
                webResponse.data = null;
            }

            return webResponse;
        }
    }
}
