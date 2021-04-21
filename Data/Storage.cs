using LeTwitchBot.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LeTwitchBot.Data
{
    internal class Storage : DbContext
    {
        public DbSet<ChannelVisitor> Visitors { get; set; }
        public DbSet<Visit> Visits { get; set; }

        public Storage()
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = data.db");
        }
    }
}
