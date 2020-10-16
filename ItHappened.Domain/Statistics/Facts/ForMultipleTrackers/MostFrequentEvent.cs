namespace ItHappened.Domain.Statistics.Facts.ForMultipleTrackers
{
    public class MostFrequentEvent : IMultipleTrackersStatisticsFact
    {
        public string Type { get; }
        public string Description { get; }
        public double Priority { get; }
        public string TrackingName { get; }
        public double EventsPeriod { get; }
        public MostFrequentEvent(string description, double priority, string trackingName, double eventsPeriod)
        {
            Description = description;
            Priority = priority;
            TrackingName = trackingName;
            EventsPeriod = eventsPeriod;
        }
    }
}