using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hidayah_collage.Models.MessageResponse
{
    [NotMapped]
    public class MessageListNotMapped
    {
        public long SEQ { get; set; }
        public string MSG_CD { get; set; }
        public string MSG_TEXT { get; set; }
        
    }
}
