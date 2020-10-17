namespace ItHappened.Domain.Statistics.Facts.ForSingleTracker
{
    public class SingleTrackerEventsCountFact : ISingleTrackerStatisticsFact
    {
        public SingleTrackerEventsCountFact(string factName, string description, double priority, int eventsCount)
        {
            FactName = factName;
            Description = description;
            Priority = priority;
            EventsCount = eventsCount;
        }

        public string FactName { get; }
        public string Description { get; }
        public double Priority { get; }
        public int EventsCount { get; }
    }
}