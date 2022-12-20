using hidayah_collage.Models;
using hidayah_collage.Models.MessageResponse;
using hidayah_collage.Models.SystemMaster;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
