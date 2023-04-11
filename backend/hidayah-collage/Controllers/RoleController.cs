using hidayah_collage.Interface;
using hidayah_collage.Models.Paging;
using hidayah_collage.Models.Roles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hidayah_collage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRole _role;

        public RoleController(IRole role)
        {
            _role = role;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRole()
        {
            if (ModelState.IsValid)
            {
                var result = await _role.GetAllRole();
                if (result.status)
                {
                    return Ok(result);
                }
            }
            return BadRequest("Some Properties are not valid ");
        }

        [HttpGet("UserAssignment")]
        public async Task<IActionResult> GetListUserByRole([FromQuery] int size = 5, int page = 1)
        {
            if (ModelState.IsValid)
            {
                var paging = new PagingRequest { Size = size, Page = page };
                var result = await _role.GetUserByRole(paging);
                if (result.status)
                {
                    return Ok(result);
                }
            }
            return BadRequest("Some Properties are not valid ");
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleRequest role)
        {
            if (ModelState.IsValid)
            {
                var result = await _role.AddRoleAsync(role);
                if (result.status)
                {
                    return Ok(result);
                }
            }
            return BadRequest("Some Properties are not valid ");
        }
        [HttpDelete("{roleName}")]
        public async Task<IActionResult> DeleteRole([FromRoute] string roleName)
        {
            if (ModelState.IsValid)
            {
                var result = await _role.DeleteRoleAsync(roleName);
                if (result.status)
                {
                    return Ok(result);
                }
            }
            return BadRequest("Some Properties are not valid ");
        }
        [HttpPost("userassignment")]
        public async Task<IActionResult> UserAssignment([FromBody] UserAssignmentRequest userAssignmentRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await _role.UserAssignment(userAssignmentRequest);
                if (result.status)
                {
                    return Ok(result);
                }
            }
            return BadRequest("Some Properties are not valid ");
        }
    }
}
