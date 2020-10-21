using System;

namespace ItHappened.Domain.Statistics
{
    public class MostEventfulWeekFact : IGeneralFact
    {
        public string FactName { get; }
        public string Description { get; }
        public double Priority { get; }
        public DateTimeOffset WeekWithLargestEventCountFirstDay;
        public DateTimeOffset WeekWithLargestEventCountLastDay;
        public int EventsCount;

        internal MostEventfulWeekFact(string type, string description, double priority,
            DateTimeOffset weekWithLargestEventCountFirstDay, DateTimeOffset weekWithLargestEventCountLastDay,
            int eventsCount)
        {
            FactName = type;
            Description = description;
            Priority = priority;
            WeekWithLargestEventCountFirstDay = weekWithLargestEventCountFirstDay;
            WeekWithLargestEventCountLastDay = weekWithLargestEventCountLastDay;
            EventsCount = eventsCount;
        }
    }
}