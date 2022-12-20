using hidayah_collage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hidayah_collage.Interface
{
    public interface IRefreshToken
    {
        Task<RefreshToken> GetByToken(string token);

        Task Create(RefreshToken refreshToken);

        Task Delete(Guid id);

        Task DeleteAll(string userId);
    }
}
