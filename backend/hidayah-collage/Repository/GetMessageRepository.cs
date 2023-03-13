using hidayah_collage.DataContext;
using hidayah_collage.Models.MessageResponse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace hidayah_collage.Repository
{
    public class GetMessageRepository
    {
        private readonly AppDbContext _appDbContext;

        public GetMessageRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public string GetMeessageText(string code)
        {
            //return _appDbContext.Message
            //    .FromSqlRaw("SELECT * FROM Message WHERE MSG_CD = {0}", code).FirstOrDefault().MSG_TEXT;
            //var key = new Message
            //{
            //    MSG_CD = code
            //};
            var data = _appDbContext.Message.AsNoTracking().FirstOrDefault(x => x.MSG_CD == code);
            if (data == null)
                return "Message Code Not Found";

            return _appDbContext.Message.Where(x => x.MSG_CD == code).AsNoTracking().FirstOrDefault().MSG_TEXT;
        }
    }
}
