using System;

namespace LeTwitchBot.Data.Models
{
    public class Visit
    {
        public int ID { get; set; }
        public DateTime VisitDate { get; set; }

        public ChannelVisitor Visitor { get; set; }

    }
}