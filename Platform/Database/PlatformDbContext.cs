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
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<User>().HasIndex(u => u.Id).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.DeactivatedAt);

            modelBuilder.Entity<ConversationLog>().HasIndex(u => u.Id).IsUnique();
            modelBuilder.Entity<ConversationLog>().HasIndex(u => u.ConversationId);
            modelBuilder.Entity<ConversationLog>().HasIndex(u => u.BotId);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {

            var changes = ChangeTracker
                .Entries()
                .Where(e =>
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified);

            foreach (var item in changes)
            {
                item.Property("CreatedAt").CurrentValue = DateTime.Now;

                if (item.State == EntityState.Added)
                {
                    item.Property("UpdatedAt").CurrentValue = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }
    }
}
