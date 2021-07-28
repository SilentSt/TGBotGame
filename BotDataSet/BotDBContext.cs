using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotDataSet
{
    public class BotDBContext : DbContext
    {
        public BotDBContext() : base()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=BotDataSet.sqlite");
        }
        public DbSet<BotUser> Users { get; set; }
        public DbSet<Friends> Friends { get; set; }
        public DbSet<Warn> Warns { get; set; }
    }
}
