using hidayah_collage.Interface;
using hidayah_collage.Models.MessageResponse;
using hidayah_collage.Models.Paging;
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
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMessage _message;

        public MessageController(IMessage message)
        {
            _message = message;
        }

        //[Authorize]
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetMessageById(int Id)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var result = await _message.GetMessageById(Id);
        //            if (result.status)
        //            {
        //                return Ok(result);
        //            }
        //            else
        //            {
        //                return BadRequest(result);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return StatusCode(500, ex.Message);
        //        }
        //    }

        //    return BadRequest("Some Properties are not valid ");
        //}
        [HttpGet("{code}")]
        public async Task<IActionResult> GetMessageByCode([FromRoute] string code)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result =  await _message.GetMessageByCode(code);
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

        [HttpGet]
        public async Task<IActionResult> GetListMessage([FromQuery] int size =5 , int page=1)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var paging = new PagingRequest { Size = size, Page = page };
                    var result = await _message.GetListMessageAsync(paging);
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

        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] CreateMessageRequest messageRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _message.CreateMessageAsync(messageRequest);
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

        [HttpPut("{code}")]
        public async Task<IActionResult> UpdateMessage([FromRoute] string code,[FromBody] UpdateProductRequest updateProductRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _message.UpdateMessageAsync(code, updateProductRequest);
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

        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteMessage([FromRoute] string code)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _message.DeleteMessageAsync(code);
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
