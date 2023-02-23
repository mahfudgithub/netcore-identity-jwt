using System.ComponentModel.DataAnnotations;

namespace hidayah_collage.Models.MessageResponse
{
    public class Message
    {
        [Key]
        public string MSG_CD { get; set; }
        public string MSG_TEXT { get; set; }
    }
}
