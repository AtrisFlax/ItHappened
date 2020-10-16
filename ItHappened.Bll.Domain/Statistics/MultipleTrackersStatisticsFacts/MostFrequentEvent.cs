namespace ItHappened.Bll.Domain.Statistics.MultipleTrackersStatisticsFacts
{
    public class MostFrequentEvent : IMultipleTrackersStatisticsFact
    {
        public MostFrequentEvent(string description, double priority, string trackingName, double eventsPeriod)
        {
            Description = description;
            Priority = priority;
            TrackingName = trackingName;
            EventsPeriod = eventsPeriod;
        }
        
        public string Description { get; }
        public double Priority { get; }
        public string TrackingName { get; }
        public double EventsPeriod { get; }
    }
}