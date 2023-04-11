using hidayah_collage.Interface;
using hidayah_collage.Models;
using hidayah_collage.Models.Exceptions;
using hidayah_collage.Models.Paging;
using hidayah_collage.Models.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hidayah_collage.Repository
{
    public class RoleRepository : IRole
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleRepository(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<WebResponse> GetAllRole()
        {
            WebResponse webResponse = new WebResponse();
            List<RolesResponse> rolesResponse = new List<RolesResponse>();

            var roles = await _roleManager.Roles.AsNoTracking().ToListAsync();

            //var roleSelect = roles.
            foreach(var role in roles)
            {
                rolesResponse.Add(new RolesResponse()
                {
                    Name = role.Name
                });
            }
            
            //if (rolesResponse.Any())
            //{
                var list = rolesResponse.OrderBy((a) => a.Name);
            //}

            webResponse.status = true;
            webResponse.message = "Success";
            webResponse.data = list;

            return webResponse;
        }
        public async Task<WebResponse> AddRoleAsync(RoleRequest role)
        {
            WebResponse webResponse = new WebResponse();

            var isExist = await IsExistRole(role.Role);

            if (!isExist)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(role.Role));
                webResponse.status = true;
                webResponse.message = "Success";
                webResponse.data = result;
            }
            else
            {
                throw new BadRequestException("Error Data Exists");
            }

            return webResponse;
        }
        private async Task<bool> IsExistRole(string role)
        {
            try
            {
                return await _roleManager.RoleExistsAsync(role);
            }
            catch(Exception ex)
            {
                throw new BadRequestException($"something went wrong : {ex.Message}");
            }
        }
        public async Task<WebResponse> GetUserByRole(PagingRequest pagingRequest)
        {
            WebResponse webResponse = new WebResponse();
            List<UserResponse> userResponses = new List<UserResponse>();
            List<UserResponse> userOrderBy = new List<UserResponse>();
            var applicationUsers = await _userManager.Users.AsNoTracking().ToListAsync();
            var roles = await _roleManager.Roles.AsNoTracking().ToListAsync();
            foreach (var user in applicationUsers)
            {
                //string[] rolesArray = (await _userManager.GetRolesAsync(user)).ToArray();
                var userRole = await _userManager.GetRolesAsync(user);

                if (userRole.Any())
                {
                    foreach (var role in userRole)
                    {
                        userResponses.Add(new UserResponse()
                        {
                            Username = user.UserName,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            //Roles = (await _userManager.GetRolesAsync(user)).ToArray()
                            Roles = role
                        });
                    }
                }
            }
            //userOrderBy =  from user in applicationUsers
            //               join role in roles on user.Id equals role.Id
            //               where user.Id != ""
            if (userResponses.Any())
            {
                userOrderBy = userResponses.OrderBy((a) => a.Username).Skip((pagingRequest.Page - 1) * pagingRequest.Size).Take(pagingRequest.Size).ToList();
            }
            webResponse.status = true;
            webResponse.message = "Success";//_getMessageRepository.GetMeessageText("SUC003");
            webResponse.data = userOrderBy;

            return webResponse;
        }

        public async Task<WebResponse> DeleteRoleAsync(string roleName)
        {
            WebResponse webResponse = new WebResponse();

            if (string.IsNullOrEmpty(roleName))
            {
                throw new BadRequestException("role name is empty");
            }

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);

                webResponse.status = true;
                webResponse.message = "Success";//_getMessageRepository.GetMeessageText("SUC003");
                webResponse.data = result;
            }
            else
            {
                throw new BadRequestException("Error Data not Exists");
            }

            return webResponse;
        }

        public async Task<WebResponse> UserAssignment(UserAssignmentRequest userAssignmentRequest)
        {
            WebResponse webResponse = new WebResponse();
            var user = new ApplicationUser();
            try
            {
                user = await _userManager.FindByIdAsync(userAssignmentRequest.UserId);
            }
            catch (Exception ex)
            {
                throw new BadRequestException($"something went wrong : {ex.Message}");
            }

            if (user != null)
            {
                var isExist = await IsExistRole(userAssignmentRequest.RoleName);

                if (isExist)
                {
                    var isInRole = await _userManager.IsInRoleAsync(user, userAssignmentRequest.RoleName);
                    if (!isInRole)
                    {
                        var result = await _userManager.AddToRoleAsync(user, userAssignmentRequest.RoleName);
                        webResponse.status = true;
                        webResponse.message = "Success";
                        webResponse.data = result;
                    }
                    else
                    {
                        throw new BadRequestException("Error user already in role");
                    }
                }
                else
                {
                    throw new BadRequestException("Error Role doesnt Exist");
                }
            }
            else
            {
                throw new InvalidException("Invalid User");
            }

            return webResponse;
        }
    }
}
