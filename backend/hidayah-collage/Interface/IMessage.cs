using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hidayah_collage.Models;
using hidayah_collage.Models.MessageResponse;
using hidayah_collage.Models.Paging;

namespace hidayah_collage.Interface
{
    public interface IMessage
    {
        Task<WebResponse> GetMessageById(int Id);
        Task<WebResponse> GetMessageByCode(string code);
        Task<WebResponse> CreateMessageAsync(CreateMessageRequest messageRequest);
        Task<MessageListResponse> GetListMessageAsync(PagingRequest pagingRequest);
        Task<WebResponse> UpdateMessageAsync(string code, UpdateProductRequest updateProductRequest);
        Task<WebResponse> DeleteMessageAsync(string code);
        Task<MessageListResponse> GetListMessageByCodeAsync(string msgCode);
    }
}
