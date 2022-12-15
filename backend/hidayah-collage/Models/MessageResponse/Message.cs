using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hidayah_collage.Models.MessageResponse
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string MSG_CD { get; set; }
        public string MSG_TEXT { get; set; }
    }
}
