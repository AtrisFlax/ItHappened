using System.Collections.Generic;

namespace ItHappened.Domain.Statistics.Facts.ForMultipleTrackers
{
    public class MostFrequentEventFact : IMultipleTrackersStatisticsFact
    {
        protected internal  MostFrequentEventFact(string factName,
            string description,
            double priority,
            IReadOnlyCollection<(EventTracker, double)> eventTrackersWithPeriods, EventTracker eventTrackerWithSmallestEventPeriod, double smallestEventsPeriod)
        {
            FactName = factName;
            Description = description;
            Priority = priority;
            EventTrackerWithSmallestEventPeriod = eventTrackerWithSmallestEventPeriod;
            EventTrackersWithPeriods = eventTrackersWithPeriods;
            SmallestEventsPeriod = smallestEventsPeriod;
        }

        public string FactName { get; }
        public string Description { get; }
        public double Priority { get; }
        public EventTracker EventTrackerWithSmallestEventPeriod  { get; }
        public IReadOnlyCollection<(string EventTitle, double EventPeriod)> EventTrackersWithPeriods { get; }
        public double SmallestEventsPeriod { get; }
    }
}