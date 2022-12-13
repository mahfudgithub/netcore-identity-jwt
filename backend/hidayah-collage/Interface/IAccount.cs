using hidayah_collage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hidayah_collage.Interface
{
    public interface IAccount
    {
        Task<WebResponse> Register(RegisterRequest registerRequest);
    }
}
