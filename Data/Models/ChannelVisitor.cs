using System;
using System.Collections.Generic;

namespace LeTwitchBot.Data.Models
{
    public class ChannelVisitor
    {
        public int ID { get; set; }
        public int TwitchID { get; set; }
        public string TwitchUsername { get; set; }
        public int Currency { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;

        public ICollection<Visit> Visits { get; set; }

        public ChannelVisitor()
        {
            
        }

        public ChannelVisitor(int twitchID, string twitchUsername)
        {
            TwitchID = twitchID;
            TwitchUsername = twitchUsername;
        }
    }
}