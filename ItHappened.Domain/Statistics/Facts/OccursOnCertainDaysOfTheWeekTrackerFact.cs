using System;
using System.Collections.Generic;
using ItHappened.Domain.Statistics;

namespace ItHappened.Domain.Statistics
{
    public class OccursOnCertainDaysOfTheWeekTrackerFact : ISingleTrackerTrackerFact
    {
        public IEnumerable<DayOfWeek> DaysOfTheWeek { get; }
        public double Percentage { get; }
        public string FactName { get; }
        public string Description { get; }
        public double Priority { get; }

        internal OccursOnCertainDaysOfTheWeekTrackerFact(string factName, string description, double priority,
            IEnumerable<DayOfWeek> daysOfTheWeek, double percentage)
        {
            FactName = factName;
            Description = description;
            Priority = priority;
            DaysOfTheWeek = daysOfTheWeek;
            Percentage = percentage;
        }
    }
}