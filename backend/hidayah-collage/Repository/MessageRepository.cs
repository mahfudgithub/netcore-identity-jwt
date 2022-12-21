using hidayah_collage.DataContext;
using hidayah_collage.Interface;
using hidayah_collage.Models;
using hidayah_collage.Models.MessageResponse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hidayah_collage.Repository
{
    public class MessageRepository : IMessage
    {
        private readonly AppDbContext _appDbContext;
        private readonly GetMessageRepository _getMessageRepository;

        public MessageRepository(AppDbContext appDbContext, GetMessageRepository getMessageRepository)
        {
            _appDbContext = appDbContext;
            _getMessageRepository = getMessageRepository;
        }

        public async Task<WebResponse> GetMessageByCode(string code)
        {
            WebResponse webResponse = new WebResponse();

            var meesage = await FindByCode(code);
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

        public async Task<WebResponse> GetMessageById(int Id)
        {
            WebResponse webResponse = new WebResponse();

            var meesage = FindById(Id);
            if (meesage != null)
            {
                var data = await _appDbContext.Message.FindAsync(Id);
                webResponse.status = true;
                webResponse.message = _getMessageRepository.GetMeessageText("SUC006");
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

        private async Task<Message> FindByCode(string Code)
        {
            //return  _appDbContext.Message.Where(x => x.MSG_CD == Code).FirstOrDefault();
            return await _appDbContext.Message.FirstOrDefaultAsync(x => x.MSG_CD == Code);
        }

        public async Task<WebResponse> CreateMessageAsync(CreateMessageRequest messageRequest)
        {
            WebResponse webResponse = new WebResponse();

            var msg = await FindByCode(messageRequest.MsgCode);
            if (msg != null)
            {
                webResponse.status = false;
                webResponse.message = _getMessageRepository.GetMeessageText("ERR007");
                webResponse.data = null;
            }
            else
            {
                var data = new Message
                {
                    MSG_CD = messageRequest.MsgCode,
                    MSG_TEXT = messageRequest.MsgText
                };

                 _appDbContext.Message.Add(data);
                var result = await _appDbContext.SaveChangesAsync();

                webResponse.status = true;
                webResponse.message = "Success Insert Data ";
                webResponse.data = result;
            }

            return webResponse;
        }

        public async Task<WebResponse> GetListMessageAsync()
        {
            WebResponse webResponse = new WebResponse();

            try
            {
                var result = await _appDbContext.Message.ToListAsync();
                if (result == null)
                {
                    webResponse.message = "No Data Found";
                }
                else
                {
                    webResponse.message = _getMessageRepository.GetMeessageText("SUC006");
                }
                webResponse.status = true;
                webResponse.data = result;
            }
            catch (Exception e)
            {
                webResponse.status = false;
                webResponse.message = e.Message.ToString();
                webResponse.data = null;
            }

            return webResponse;
        }

        public async Task<WebResponse> UpdateMessageAsync(string code, UpdateProductRequest updateProductRequest)
        {
            WebResponse webResponse = new WebResponse();

            if (string.IsNullOrEmpty(code))
            {
                webResponse.status = false;
                webResponse.message = "Message Code Should not be empty";
                webResponse.data = null;

                return webResponse;
            }

            var msg = await FindByCode(code);
            if (msg == null)
            {
                webResponse.status = false;
                webResponse.message = "No Data Found";
                webResponse.data = null;

                return webResponse;
            }

            var data = new Message
            {
                MSG_CD = code,
                MSG_TEXT = updateProductRequest.MsgText
            };

            _appDbContext.Message.Update(data);
            var result = await _appDbContext.SaveChangesAsync();
            webResponse.status = true;
            webResponse.message = "Success Update Data ";
            webResponse.data = result;

            return webResponse;
        }

        public async Task<WebResponse> DeleteMessageAsync(string code)
        {
            WebResponse webResponse = new WebResponse();

            var msg = await FindByCode(code);
            if (msg == null)
            {
                webResponse.status = false;
                webResponse.message = "No Data Found";
                webResponse.data = null;
            }
            else
            {
                var data = new Message
                {
                    MSG_CD = code
                };
                _appDbContext.Message.Remove(data);
                var result = await _appDbContext.SaveChangesAsync();

                webResponse.status = true;
                webResponse.message = "Success Delete Data";
                webResponse.data = result;
            }

            return webResponse;
        }
    }
}
