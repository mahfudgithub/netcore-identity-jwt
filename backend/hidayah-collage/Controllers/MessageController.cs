using hidayah_collage.Interface;
using Microsoft.AspNetCore.Authorization;
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
    public class MessageController : ControllerBase
    {
        private readonly IMessage _message;

        public MessageController(IMessage message)
        {
            _message = message;
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessageById(int Id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _message.GetMessageById(Id);
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
    }
}
