using hidayah_collage.DataContext;
using hidayah_collage.Interface;
using hidayah_collage.Models;
using hidayah_collage.Models.Email;
using hidayah_collage.Models.TokenGenerator;
using hidayah_collage.Models.TokenValidator;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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

        public AccountRepository(UserManager<ApplicationUser> userManager
            ,SignInManager<ApplicationUser> signInManager
            ,IConfiguration configuration
            ,IMailService mailService
            ,GetMessageRepository getMessageRepository
            ,AccessTokenGenerator accessTokenGenerator
            ,RefreshTokenGenerator refreshTokenGenerator
            ,IRefreshToken refreshToken
            ,RefreshTokenValidator refreshTokenValidator
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
        }

        public async Task<WebResponse> Login(LoginRequest loginRequest)
        {
            WebResponse webResponse = new WebResponse();
            var response = new LoginResponse();
            ApplicationUser applicationUser = new ApplicationUser();
            //var email = new EmailAddressAttribute();

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

            var result = await _signInManager.PasswordSignInAsync(applicationUser.UserName, loginRequest.Password,false,false);

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

            response.FirstName = applicationUser.FirstName;
            response.LastName = applicationUser.LastName;
            response.Username = applicationUser.Email;
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

            var emailAccount = ValidateEmail(registerRequest.Email);
            if (emailAccount.Result != null)
            {
                var messageTxt = _getMessageRepository.GetMeessageText("ERR001");
                webResponse.status = false;
                webResponse.message = messageTxt;
                webResponse.data = null;
                return webResponse;
            }

            var usernameAccount = ValidateUserName(registerRequest.UserName);
            if (usernameAccount.Result != null)
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

            var user = ValidateEmail(forgotPasswordRequest.Email);
            if (user.Result == null)
            {
                webResponse.status = false;
                webResponse.message = _getMessageRepository.GetMeessageText("ERR002");
                webResponse.data = null;
                return webResponse;
            }

            try
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user.Result);

                var encodedToken = Encoding.UTF8.GetBytes(code);
                var validToken = WebEncoders.Base64UrlEncode(encodedToken);

                //var callbackUrl = new Uri($"{_configuration["AppClientUrl"]}"+@"/ResetPassword?email=" + user.Result.Email + "&token=" + validToken);
                var callbackUrl = new Uri($"{_configuration["AppClientUrl"]}" + @"/account/resetpassword?email=" + user.Result.Email + "&token=" + validToken);
                string subject = "Reset Password";
                string targetBody = "_blank";
                string rel = "noopener noreferrer";
                string body = "Please reset your password by clicking <a target=\"" + targetBody + "\" rel=\""+rel+"\" href=\"" + callbackUrl + "\"> Clicking here</a>";

                emailRequest.Subject = subject;
                emailRequest.ToEmail = user.Result.Email;
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
                webResponse.message = ex.ToString();
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

            return webResponse;

        }

        public async Task<WebResponse> ConfirmEmailAsync(string userId, string token)
        {
            WebResponse webResponse = new WebResponse();
            //var response = new ResetPasswordResponse();

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

            return webResponse;
        }

        public async Task<WebResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            WebResponse webResponse = new WebResponse();
            var response = new LoginResponse();
            RefreshToken refreshTokenDTO = new RefreshToken();

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

            response.FirstName = user.FirstName;
            response.LastName = user.LastName;
            response.Username = user.Email;
            response.Token = generateToken.Token;
            response.ExpireDate = generateToken.ExpireDate;
            response.RefreshToken = refreshToken;

            webResponse.status = true;
            webResponse.message = _getMessageRepository.GetMeessageText("SUC005");
            webResponse.data = response;

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
    }
}
