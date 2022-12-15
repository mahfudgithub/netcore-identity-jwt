using hidayah_collage.DataContext;
using hidayah_collage.Interface;
using hidayah_collage.Models;
using hidayah_collage.Models.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace hidayah_collage.Repository
{
    public class AccountRepository : IAccount
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        private readonly IOptions<EmailConfig> _options;
        private readonly IMailService _mailService;

        public AccountRepository(UserManager<ApplicationUser> userManager
            ,AppDbContext appDbContext
            ,IConfiguration configuration
            ,IOptions<EmailConfig> options
            ,IMailService mailService)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
            _configuration = configuration;
            _options = options;
            _mailService = mailService;
        }

        public async Task<WebResponse> Login(LoginRequest loginRequest)
        {
            WebResponse webResponse = new WebResponse();
            var response = new LoginResponse();

            var emailAccount = ValidateEmail(loginRequest.Email);
            if (emailAccount.Result == null)
            {
                webResponse.status = false;
                webResponse.message = "There is no user with that Email address";
                webResponse.data = null;
                return webResponse;
            }

            var result = await _userManager.CheckPasswordAsync(emailAccount.Result, loginRequest.Password);

            if (!result)
            {
                webResponse.status = false;
                webResponse.message = "Invalid Password";
                webResponse.data = null;
                return webResponse;
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginRequest.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var authSignInKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256Signature)
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
                webResponse.message = "Success";
                webResponse.data = response;
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
                UserName = registerRequest.Email
            };

            if(registerRequest.Password != registerRequest.ConfirmPassword)
            {
                webResponse.status = false;
                webResponse.message = "Password and ConfirmPassword do not match.";
                webResponse.data = null;
                return webResponse;
            }

            var emailAccount = ValidateEmail(registerRequest.Email);
            if (emailAccount.Result != null)
            {
                var messageTxt = GetMeessageText("ERR001");
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
                await SendEmailAsync(emailRequest);

                response.FirstName = registerRequest.FirstName;
                response.Username = registerRequest.Email;
                response.Email = registerRequest.Email;
                webResponse.status = true;
                webResponse.message = GetMeessageText("SUC001");
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

        public string GetMeessageText(string code)
        {
            var msgTxt = _appDbContext.Message
                .FromSqlRaw("SELECT * FROM Message WHERE MSG_CD = {0}",code).FirstOrDefault();

            return msgTxt.MSG_TEXT;
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
                webResponse.message = "There is no user with that Email address";
                webResponse.data = null;
                return webResponse;
            }

            try
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user.Result);

                var encodedToken = Encoding.UTF8.GetBytes(code);
                var validToken = WebEncoders.Base64UrlEncode(encodedToken);

                var callbackUrl = new Uri($"{_configuration["AppUrl"]}"+@"/ResetPassword?email=" + user.Result.Email + "&token=" + validToken);

                string subject = "Reset Password";
                string body = "Please reset your password by clicking <a href=\"" + callbackUrl + "\">Clicking here</a>";

                emailRequest.Subject = subject;
                emailRequest.ToEmail = user.Result.Email;
                emailRequest.Body = body;
                //var mail = _mailService.SendEmailAsync(emailRequest);
                await SendEmailAsync(emailRequest);
                
                response.status = true;
                response.body = body;
                webResponse.status = true;
                webResponse.message = GetMeessageText("SUC002");
                webResponse.data = response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return webResponse;
        }

        public async Task SendEmailAsync(EmailRequest emailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_options.Value.Mail);
            email.To.Add(MailboxAddress.Parse(emailRequest.ToEmail));
            email.Subject = emailRequest.Subject;
            var builder = new BodyBuilder();
            if (emailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in emailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = emailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_options.Value.Host, _options.Value.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Value.Mail, _options.Value.Password);
            var status = await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task<WebResponse> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            WebResponse webResponse = new WebResponse();
            var response = new ResetPasswordResponse();

            if (resetPasswordRequest.NewPassword != resetPasswordRequest.ConfirmNewPassword)
            {
                webResponse.status = false;
                webResponse.message = "Password and ConfirmPassword do not match.";
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
                    webResponse.message = GetMeessageText("SUC001");
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
                var messageTxt = GetMeessageText("ERR001");
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
                    webResponse.message = GetMeessageText("SUC001");
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
                webResponse.message = "User not Found";
                webResponse.data = null;
            }

            return webResponse;
        }
    }
}
