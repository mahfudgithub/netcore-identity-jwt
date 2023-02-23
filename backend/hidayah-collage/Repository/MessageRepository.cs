using hidayah_collage.DataContext;
using hidayah_collage.Interface;
using hidayah_collage.Models;
using hidayah_collage.Models.MessageResponse;
using hidayah_collage.Models.Paging;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Sakura.AspNetCore;
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
                webResponse.message = _getMessageRepository.GetMeessageText("SUC010");
                webResponse.data = data;
            }

            return webResponse;
        }

        public async Task<MessageListResponse> GetListMessageAsync(PagingRequest pagingRequest)
        {
            MessageListResponse messageListResponse = new MessageListResponse();

            try
            {
                var rowStart = 1;
                var rowEnd = 5;
                rowStart = (pagingRequest.Page - 1) * pagingRequest.Size + 1;
                rowEnd = rowStart + pagingRequest.Size - 1;
                var resultCount = _appDbContext.Message.Count();
                //Sql Raw
                //var result = await _appDbContext.messageListNotMappeds.FromSqlRaw("select * from (SELECT ROW_NUMBER() over(order by [MSG_CD] asc ) [SEQ],[MSG_CD],[MSG_TEXT] FROM [dbo].[Message])tb where 1 = 1 and tb.SEQ between {0} and {1}", rowStart, rowEnd).ToListAsync();
                //SP
                //var result = await _appDbContext.messageListNotMappeds.FromSqlRaw("dbo.[GetMessageList] @rowStart,@rowEnd", pRowStart, pRowEnd).ToListAsync();
                //var result = await _appDbContext.messageListNotMappeds.FromSqlRaw("dbo.[GetMessageList] @rowStart,@rowEnd", pRowStart, pRowEnd).ToListAsync();
                
                var result = await GetMessageListsAsync(rowStart, rowEnd);
               
                //var result1 = await _appDbContext.Set<MessageListNotMapped>().FromSqlRaw("Exec dbo.GetMessageList @rowStart,@rowEnd",  pRowStart, pRowEnd ).ToArrayAsync();
                //var result = _appDbContext.Set<MessageListNotMapped>().FromSql("dbo.[GetMessageList] @rowStart,@rowEnd", pRowStart, pRowEnd).ToList();
         
                //var db = new AppDbContext();
                //string query = @"select ID , Name from People where ... ";
                //var result = db.ExecuteQuery<MessageListNotMapped>(query);

                //In EntityFrameworkCore we have two methods for executing Stored Procedures:
                //1.FromSqlRaw() – used to run query statements that return records from the database
                //2. ExecuteSqlRaw() / ExecuteSqlRawAsync() – executes a command that can modify data on the database(typically DML commands like INSERT, UPDATE or DELETE)
                //var userIdParam = new SqlParameter("@Id", SqlDbType.Int);
                //userIdParam.Direction = ParameterDirection.Output;
                //this is exec for DML (Insert ,update,delete)
                //var result1 = await _appDbContext.Database.ExecuteSqlRawAsync("exec dbo.[GetMessageList] @rowStart,@rowEnd, @Id out", parameters: new[] { pRowStart1, pRowEnd2 , userIdParam });
                if (result == null)
                {
                    messageListResponse.message = "No Data Found";
                }
                else
                {
                    messageListResponse.message = _getMessageRepository.GetMeessageText("SUC006");
                }
                messageListResponse.status = true;
                //messageListResponse.data.List = result.ToPagedList(pagingRequest.Size,pagingRequest.Page);
                messageListResponse.data.List = result;
                messageListResponse.data.total = resultCount;
            }
            catch (Exception e)
            {
                messageListResponse.status = false;
                messageListResponse.message = e.Message.ToString();
                messageListResponse.data = null;
            }

            return messageListResponse;
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
            webResponse.message = _getMessageRepository.GetMeessageText("SUC008");
            webResponse.data = data;

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
                webResponse.message = _getMessageRepository.GetMeessageText("SUC009");
                webResponse.data = null;
            }

            return webResponse;
        }

        public async Task<MessageListResponse> GetListMessageByCodeAsync(string msgCode)
        {
            MessageListResponse messageListResponse = new MessageListResponse();

            try
            {
                //SqlParameter paramMsgCode = new SqlParameter("@msgCode", msgCode);
                var result = await GetMessageByCodeAsync(msgCode);//_appDbContext.messageListNotMappeds.FromSqlRaw("dbo.[GetMessageListByCode] @msgCode", paramMsgCode).ToListAsync();


                if (result == null)
                {
                    messageListResponse.message = "No Data Found";
                }
                else
                {
                    messageListResponse.message = _getMessageRepository.GetMeessageText("SUC006");
                }
                messageListResponse.status = true;
                messageListResponse.data.List = result.ToPagedList(5,1);
                messageListResponse.data.total = result.Count();

            }
            catch (Exception e)
            {
                messageListResponse.status = false;
                messageListResponse.message = e.Message.ToString();
                messageListResponse.data = null;
            }

            return messageListResponse;
        }

        public async Task<IEnumerable<MessageListNotMapped>> GetMessageListsAsync(int rowStart, int rowEnd)
        {
            
            object[] myParms =
                {   new SqlParameter("@rowStart", rowStart),
                    new SqlParameter("@rowEnd", rowEnd)
                };

            return await _appDbContext.Set<MessageListNotMapped>()
                     .FromSqlRaw("Execute dbo.[GetMessageList] @rowStart,@rowEnd", myParms).AsNoTracking()
                    .ToListAsync();
        }

        public async Task<IEnumerable<MessageListNotMapped>> GetMessageByCodeAsync(string msgCode)
        {
            object[] myParms =
                {   new SqlParameter("@msgCode", msgCode)
                };

            return await _appDbContext.Set<MessageListNotMapped>()
                     .FromSqlRaw("dbo.[GetMessageListByCode] @msgCode", myParms).AsNoTracking()
                    .ToListAsync();
        }

        //public IQueryable<MessageListNotMapped> SearchMessageLists(int rowStart, int rowEnd)
        //{
        //    //var pRowStart = new SqlParameter("@rowStart", rowStart);
        //    //var pRowEnd = new SqlParameter("@rowEnd", rowEnd);

        //    object[] myParms =
        //        {   new SqlParameter("@rowStart", rowStart),
        //            new SqlParameter("@rowEnd", rowEnd)
        //        };

        //    return this.messageListNotMappeds.FromSqlRaw("Execute dbo.[GetMessageList] @rowStart,@rowEnd", myParms);
        //}

    }
}
