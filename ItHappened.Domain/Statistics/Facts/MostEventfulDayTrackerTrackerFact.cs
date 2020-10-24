﻿using System;

namespace ItHappened.Domain.Statistics
{
    public class MostEventfulDayTrackerTrackerFact : IMultipleTrackerTrackerFact
    {
        public string FactName { get; }
        public string Description { get; }
        public double Priority { get; }
        public DateTimeOffset DayWithLargestEventsCount { get; }
        public int EventsCount { get; }

        internal MostEventfulDayTrackerTrackerFact(string type, string description, double priority,
            DateTimeOffset dayWithLargestEventsCount, int eventsCount)
        {
            FactName = type;
            Description = description;
            Priority = priority;
            DayWithLargestEventsCount = dayWithLargestEventsCount;
            EventsCount = eventsCount;
        }
    }
}