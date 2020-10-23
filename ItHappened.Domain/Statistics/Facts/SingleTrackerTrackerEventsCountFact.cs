﻿namespace ItHappened.Domain.Statistics
{
    public class SingleTrackerTrackerEventsCountFact : ISingleTrackerTrackerFact
    {
        public int EventsCount { get; }
        public string FactName { get; }
        public string Description { get; }
        public double Priority { get; }

        internal SingleTrackerTrackerEventsCountFact(string factName, string description, double priority,
            int eventsCount)
        {
            FactName = factName;
            Description = description;
            Priority = priority;
            EventsCount = eventsCount;
        }
    }
}