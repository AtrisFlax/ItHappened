using System.Collections.Generic;

namespace ItHappened.Domain.Statistics.Facts.ForMultipleTrackers
{
    public class MostFrequentEvent : IMultipleTrackersStatisticsFact
    {
        public MostFrequentEvent(double priority,
            IReadOnlyCollection<(EventTracker, double)> eventTrackersWithPeriods,
            EventTracker eventTrackerWithSmallestEventPeriod,
            double smallestEventsPeriod)
        {
            Priority = priority;
            EventTrackerWithSmallestEventPeriod = eventTrackerWithSmallestEventPeriod;
            EventTrackersWithPeriods = eventTrackersWithPeriods;
            SmallestEventsPeriod = smallestEventsPeriod;
        }

        public string FactName { get; }
        public string Description => $"Чаще всего у вас происходит событие " +
                                     $"{EventTrackerWithSmallestEventPeriod.Name}" +
                                     $" - раз в {SmallestEventsPeriod} дней";
        public double Priority { get; }
        public EventTracker EventTrackerWithSmallestEventPeriod  { get; }
        public IReadOnlyCollection<(EventTracker, double)> EventTrackersWithPeriods { get; }
        public double SmallestEventsPeriod { get; }
    }
}