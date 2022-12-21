using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hidayah_collage.Models.MessageResponse
{
    public class UpdateProductRequest
    {
        [StringLength(200)]
        public string MsgText { get; set; }
    }
}
