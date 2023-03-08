using hidayah_collage.Interface;
using hidayah_collage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace hidayah_collage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccount _account;
        private readonly IConfiguration _configuration;

        public AccountController(IAccount account, IConfiguration configuration)
        {
            _account = account;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest registerRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _account.Register(registerRequest);
                    if (result.status)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return Ok(result);
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }

            return BadRequest("Some Properties are not valid ");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest loginRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _account.Login(loginRequest);                    
                    if (result.status)
                    {
                        string token = (string)result.data.GetType().GetProperty("RefreshToken").GetValue(result.data);
                        Response.Cookies.Append("refreshToken", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict,MaxAge= TimeSpan.FromDays(2)});
                        return Ok(result);
                    }
                    else
                    {
                        return Ok(result);
                    }
                }
                catch (Exception)
                {
                    return StatusCode(500);
                }
            }

            return BadRequest("Some Properties are not valid ");
        }

        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordRequest forgotPasswordRequest)
        {
            if (string.IsNullOrEmpty(forgotPasswordRequest.Email))
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _account.ForgotPassword(forgotPasswordRequest);
                    if (result.status)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return Ok(result);
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(417, ex.Message);
                }
            }
            return BadRequest("Some Properties are not valid ");
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest resetPasswordRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _account.ResetPassword(resetPasswordRequest);
                    if (result.status)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return Ok(result);
                    }
                }
                catch (Exception)
                {
                    return StatusCode(500);
                }
            }
            return BadRequest("Some Properties are not valid ");
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                return NotFound();

            var result = await _account.ConfirmEmailAsync(userId, token);

            if (result.status)
            {
                //return Redirect($"{_configuration["AppUrl"]}/confirmemail.html");
                return Redirect($"{_configuration["AppClientUrl"]}/confirmemail");
            }

            return BadRequest(result);

        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _account.RefreshToken(refreshTokenRequest);
                    if (result.status)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return Ok(result);
                    }
                }
                catch (Exception)
                {
                    return StatusCode(500);
                }
            }
            return BadRequest("Some Properties are not valid ");
        }

        [Authorize]
        [HttpDelete("logout")]
        public async Task<IActionResult> Logout()
        {
            string rawUserId = HttpContext.User.FindFirstValue("id");
            try
            {
                var result = await _account.Logout(rawUserId);
                if (result.status)
                {                    
                    Response.Cookies.Delete("refreshToken");
                    return NoContent();
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            //Response.Headers.Remove("Authorization");
           // HttpContext.Session.Clear();
            
            //return Redirect($"{_configuration["AppUrl"]}/confirmemail.html");
        }

        [Authorize]
        [HttpGet("profile/{id}")]
        public async Task<IActionResult> GetUserInfo([FromRoute] string id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _account.GetUserInfo(id);
                    if (result.status)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return Ok(result);
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }

            return BadRequest("Some Properties are not valid ");
        }

        [Authorize]
        [HttpPost("confirmedemail/{id}")]
        public async Task<IActionResult> SendEmailConfirmed([FromRoute] string id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _account.SendConfirmedEmail(id);
                    if (result.status)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return Ok(result);
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }

            return BadRequest("Some Properties are not valid ");
        }

        public IActionResult BadRequestResult()
        {
            IEnumerable<string> errMsg = ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage));

            return BadRequest(errMsg);
        }
    }
}
