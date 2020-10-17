using ItHappened.Domain.Statistics.Calculators.ForMultipleTrackers;
using ItHappened.Domain.Statistics.Facts.ForMultipleTrackers;

namespace ItHappened.Domain.Statistics.Facts.ForSingleTracker
{
    public class EventsCountFact : IMultipleTrackersStatisticsFact
    {
        public EventsCountFact(double priority, int eventsCount)
        {
            Priority = priority;
            EventsCount = eventsCount;
        }

        public string FactName { get; } 
        public string Description  => $"У вас произошло уже {EventsCount} событий!";
        public double Priority { get; }
        public int EventsCount { get; }
    }
}