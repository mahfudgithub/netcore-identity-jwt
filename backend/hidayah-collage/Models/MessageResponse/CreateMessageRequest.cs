using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hidayah_collage.Models.MessageResponse
{
    public class CreateMessageRequest
    {
        [Required, StringLength(10)]
        public string MsgCode { get; set; }        
        [StringLength(200)]
        public string MsgText { get; set; }
    }
}
