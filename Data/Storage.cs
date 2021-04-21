using System;
using System.Collections;
using System.Collections.Generic;
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

    public class ChannelVisitor
    {
        public int ID { get; set; }
        public int TwitchID { get; set; }
        public string TwitchUsername { get; set; }
        public ICollection<Visit> Visits { get; set; }
    }


    public class Visit
    {
        public int ID { get; set; }
        public DateTime VisitDate { get; set; }

        public ChannelVisitor Visitor { get; set; }

    }
}
