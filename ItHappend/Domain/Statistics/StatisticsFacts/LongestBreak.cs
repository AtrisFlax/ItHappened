﻿namespace ItHappend.Domain.Statistics.StatisticsFacts
{
    public class LongestBreak : IStatisticsFact
    {
        public LongestBreak(string description,
            double priority,
            int durationInDays,
            Event lastEventBeforeBreakDate,
            Event firstEventAfterBreakDate)
        {
            Description = description;
            Priority = priority;
            DurationInDays = durationInDays;
            LastEventBeforeBreakDate = lastEventBeforeBreakDate;
            FirstEventAfterBreakDate = firstEventAfterBreakDate;
        }
        
        public string Description { get; }
        public double Priority { get; }
        public int DurationInDays { get; }
        public Event LastEventBeforeBreakDate { get; }
        public Event FirstEventAfterBreakDate { get; }
    }
}