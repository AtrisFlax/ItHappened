using System;

namespace ItHappened.Domain.Statistics.Facts.ForMultipleTrackers
{
    public class MultipleTrackersEventsCountFact : IMultipleTrackersStatisticsFact
    {
        public MultipleTrackersEventsCountFact(int eventsCount)
        {
            EventsCount = eventsCount;
        }

        public string FactName { get; } 
        public string Description  => $"У вас произошло уже {EventsCount} событий!";
        public double Priority => Math.Log(EventsCount);
        public int EventsCount { get; }
    }
}