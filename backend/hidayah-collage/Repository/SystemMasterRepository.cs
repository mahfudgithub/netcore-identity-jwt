using hidayah_collage.DataContext;
using hidayah_collage.Interface;
using hidayah_collage.Models;
using hidayah_collage.Models.SystemMaster;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hidayah_collage.Repository
{
    public class SystemMasterRepository : ISystemMaster
    {
        private readonly AppDbContext _appDbContext;
        private readonly GetMessageRepository _getMessageRepository;

        public SystemMasterRepository(AppDbContext appDbContext, GetMessageRepository getMessageRepository)
        {
            _appDbContext = appDbContext;
            _getMessageRepository = getMessageRepository;
        }

        public async Task<List<SystemMaster>> GetListMasterByType(string type)
        {
            //List<SystemMaster> sysMaterList = new List<SystemMaster>();
            object[] myParms =
            {
                new SqlParameter("@type", type)
            };

            List<SystemMaster> sysMaterList = await _appDbContext.Set<SystemMaster>().FromSqlRaw("dbo.[GetSystemByType] @type", myParms).AsNoTracking().ToListAsync();

            return sysMaterList;
        }

        public async Task<WebResponse> GetSystemMasterByType(string type)
        {
            WebResponse webResponse = new WebResponse();

            var meesage = await FindByType(type);
            if (meesage != null)
            {
                webResponse.status = true;
                webResponse.message = _getMessageRepository.GetMeessageText("SUC006");
                webResponse.data = meesage;
            }
            else
            {
                webResponse.status = false;
                webResponse.message = "Error retrieve data ";
                webResponse.data = null;
            }

            return webResponse;
        }

        private async Task<SystemMaster> FindByType(string type)
        {
            return await _appDbContext.System.FirstOrDefaultAsync(x => x.Type == type);
        }
    }
}
