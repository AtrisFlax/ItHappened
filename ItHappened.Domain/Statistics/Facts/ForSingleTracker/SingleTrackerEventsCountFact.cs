using System;

namespace ItHappened.Domain.Statistics.Facts.ForSingleTracker
{
    public class SingleTrackerEventsCountFact : ISingleTrackerStatisticsFact
    {
        public SingleTrackerEventsCountFact(EventTracker targetEventTracker, int eventsCount)
        {
            TargetEventTracker = targetEventTracker;
            EventsCount = eventsCount;
        }

        public string FactName { get; }
        public EventTracker TargetEventTracker { get; }
        public string Description  => $"Событие {TargetEventTracker.Name} произошло {EventsCount} раз";
        public double Priority => Math.Log(EventsCount);
        public int EventsCount { get; }
    }
}