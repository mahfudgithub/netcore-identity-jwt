using hidayah_collage.DataContext;
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
            return _appDbContext.Message
                .FromSqlRaw("SELECT * FROM Message WHERE MSG_CD = {0}", code).FirstOrDefault().MSG_TEXT;

        }
    }
}
