using System;
using System.Collections.Generic;
using ItHappened.Domain.Statistics.Facts.ForSingleTracker;

namespace ItHappend.Domain.Statistics.StatisticsFacts
{
    public class OccursOnCertainDaysOfTheWeekFact : ISingleTrackerStatisticsFact
    {
        public string Type { get; }
        public string Description { get; }
        public double Priority { get; }
        public IEnumerable<DayOfWeek> DaysOfTheWeek { get; }
        public double Percentage { get; }
        
        public OccursOnCertainDaysOfTheWeekFact(string type, string description, double priority, IEnumerable<DayOfWeek> daysOfTheWeek, double percentage)
        {
            Type = type;
            Description = description;
            Priority = priority;
            DaysOfTheWeek = daysOfTheWeek;
            Percentage = percentage;
        }
    }
}