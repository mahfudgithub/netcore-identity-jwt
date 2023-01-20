using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hidayah_collage.Models.MessageResponse
{
    public class MessageListResponse
    {
        public bool status { get; set; }
        public MessageResponse data { get; set; }
        public string message { get; set; }
        public MessageListResponse()
        {
            data = new MessageResponse();
        }

        public class MessageResponse
        {
            public int total { get; set; }
            public object List { get; set; }
        }
    }
}
