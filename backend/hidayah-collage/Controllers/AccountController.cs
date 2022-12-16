using hidayah_collage.Interface;
using hidayah_collage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
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
                        return BadRequest(result);
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
                        return Ok(result);
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
                        return BadRequest(result);
                    }
                }
                catch (Exception)
                {
                    return StatusCode(500);
                }
            }
            return BadRequest("Some Properties are not valid ");
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync([FromForm] ResetPasswordRequest resetPasswordRequest)
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
                        return BadRequest(result);
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
                return Redirect($"{_configuration["AppUrl"]}/confirmemail.html");
            }

            return BadRequest(result);

        }

        [HttpPost]
        public IActionResult Logout()
        {
            Response.Headers.Remove("Authorization");
            Response.Cookies.Delete("");
            HttpContext.Session.Clear();
            return Redirect($"{_configuration["AppUrl"]}/confirmemail.html");
        }

        public IActionResult BadRequestResult()
        {
            IEnumerable<string> errMsg = ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage));

            return BadRequest(errMsg);
        }
    }
}
