using hidayah_collage.Models;
using hidayah_collage.Models.MessageResponse;
using hidayah_collage.Models.SystemMaster;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace hidayah_collage.DataContext
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<Message> Message { get; set; }
        public DbSet<SystemMaster> System { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        
        public DbSet<MessageListNotMapped> messageListNotMappeds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MessageListNotMapped>(builder =>
            {
                builder.ToView("MessageTableTemp", "dbo"); //for exclude from migration 3.1
            });
            modelBuilder.Entity<MessageListNotMapped>().HasNoKey();

            modelBuilder.Entity<SystemMaster>(builder =>
            {
                builder.HasKey(c => new { c.Type, c.Code });
            });

           
            base.OnModelCreating(modelBuilder);
            
        }
    }
}
