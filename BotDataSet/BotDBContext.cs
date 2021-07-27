using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotDataSet
{
    class BotDBContext : DbContext
    {
        public BotDBContext() : base()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=BotDataSet.sqlite");
        }
        public DbSet<BotUser> Users { get; set; }
    }
}
