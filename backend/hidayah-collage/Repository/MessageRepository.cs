using hidayah_collage.DataContext;
using hidayah_collage.Interface;
using hidayah_collage.Models;
using hidayah_collage.Models.MessageResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hidayah_collage.Repository
{
    public class MessageRepository : IMessage
    {
        private readonly AppDbContext _appDbContext;

        public MessageRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<WebResponse> GetMessageById(int Id)
        {
            WebResponse webResponse = new WebResponse();

            var meesage = FindById(Id);
            if (meesage != null)
            {
                var data = await _appDbContext.Message.FindAsync(Id);
                webResponse.status = true;
                webResponse.message = "Success retrieve Data ";
                webResponse.data = data;
            }
            else
            {
                webResponse.status = false;
                webResponse.message = "Error retrieve data ";
                webResponse.data = null;
            }

            return webResponse;
        }

        private Message FindById(int Id)
        {
            return _appDbContext.Message.Find(Id);
        }
    }
}
