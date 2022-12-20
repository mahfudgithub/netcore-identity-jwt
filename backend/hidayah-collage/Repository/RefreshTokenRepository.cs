using hidayah_collage.DataContext;
using hidayah_collage.Interface;
using hidayah_collage.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hidayah_collage.Repository
{
    public class RefreshTokenRepository : IRefreshToken
    {
        private readonly AppDbContext _appDbContext;

        public RefreshTokenRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Create(RefreshToken refreshToken)
        {
            _appDbContext.RefreshTokens.Add(refreshToken);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<RefreshToken> GetByToken(string token)
        {
            return await _appDbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
        }
        public async Task Delete(Guid id)
        {
            RefreshToken refreshToken = await _appDbContext.RefreshTokens.FindAsync(id);
            if (refreshToken != null)
            {
                _appDbContext.RefreshTokens.Remove(refreshToken);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAll(string userId)
        {
            IEnumerable<RefreshToken> refreshTokens = await _appDbContext.RefreshTokens
                .Where(t => t.UserId == userId)
                .ToListAsync();

            _appDbContext.RefreshTokens.RemoveRange(refreshTokens);
            await _appDbContext.SaveChangesAsync();
        }

    }
}
