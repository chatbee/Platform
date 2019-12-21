using Microsoft.EntityFrameworkCore;
using Platform.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform.Database
{
    public class PlatformDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ConversationLog> ConversationLogs { get; set; }

        public PlatformDbContext(DbContextOptions options): base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Id).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email);

            modelBuilder.Entity<ConversationLog>().HasIndex(u => u.Id).IsUnique();
            modelBuilder.Entity<ConversationLog>().HasIndex(u => u.ConversationId);
            modelBuilder.Entity<ConversationLog>().HasIndex(u => u.BotId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
