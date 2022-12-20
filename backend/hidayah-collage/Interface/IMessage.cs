using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hidayah_collage.Models;

namespace hidayah_collage.Interface
{
    public interface IMessage
    {
        Task<WebResponse> GetMessageById(int Id);
    }
}
