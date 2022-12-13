using hidayah_collage.Interface;
using hidayah_collage.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hidayah_collage.Repository
{
    public class AccountRepository : IAccount
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<WebResponse> Register(RegisterRequest registerRequest)
        {
            WebResponse webResponse = new WebResponse();
            var response = new RegisterResponse();
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

            var result = await _userManager.CreateAsync(user, registerRequest.Password);
            
            if (result.Succeeded)
            {
                response.FirstName = registerRequest.FirstName;
                response.Username = registerRequest.Email;
                response.Email = registerRequest.Email;
                webResponse.status = true;
                webResponse.message = "Succesfully Register";
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
    }
}
