using System.Collections.Generic;

namespace LeTwitchBot.Data.Models
{
    public class ChannelVisitor
    {
        public int ID { get; set; }
        public int TwitchID { get; set; }
        public string TwitchUsername { get; set; }
        public ICollection<Visit> Visits { get; set; }
    }
}