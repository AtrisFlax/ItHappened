namespace ItHappened.Domain.Statistics
{
    public class AverageRatingTrackerFact : ISingleTrackerTrackerFact
    {
        public double AverageRating { get; }
        public string FactName { get; }
        public string Description { get; }
        public double Priority { get; }

        internal AverageRatingTrackerFact(string type, string description, double priority, double averageRating)
        {
            FactName = type;
            Description = description;
            Priority = priority;
            AverageRating = averageRating;
        }
    }
}