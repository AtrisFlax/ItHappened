namespace ItHappened.Domain.Statistics.Facts.ForSingleTracker
{
    public class AverageRatingFact : ISingleTrackerStatisticsFact
    {
        public AverageRatingFact(EventTracker targetEventTracker,
            string description,
            double priority,
            double averageRating)
        {
            TargetEventTracker = targetEventTracker;
            Description = description;
            Priority = priority;
            AverageRating = averageRating;
        }
        
        public string FactName { get; }
        public EventTracker TargetEventTracker { get; }
        public string Description { get; }
        public double Priority { get; }
        public double AverageRating { get; }
    }
}