using hidayah_collage.Models;
using hidayah_collage.Models.SystemMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hidayah_collage.Interface
{
    public interface ISystemMaster
    {
        Task<WebResponse> GetSystemMasterByType(string type);
        Task<List<SystemMaster>> GetListMasterByType(string type);
    }
}
