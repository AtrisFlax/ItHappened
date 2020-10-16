namespace ItHappened.Domain.Statistics.Facts.ForSingleTracker
{
    public class AverageRatingFact : ISingleTrackerStatisticsFact
    {
        public string FactName { get; }
        public string Description { get; }
        public double Priority { get; }
        public double AverageRating { get; }

        public AverageRatingFact(string type, string description, double priority, double averageRating)
        {
            FactName = type;
            Description = description;
            Priority = priority;
            AverageRating = averageRating;
        }
    }
}