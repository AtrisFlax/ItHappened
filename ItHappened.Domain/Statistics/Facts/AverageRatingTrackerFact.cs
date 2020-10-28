namespace ItHappened.Domain.Statistics
{
    public class AverageRatingTrackerFact : ISingleTrackerFact
    {
        public double AverageRating { get; }
        public string FactName { get; }
        public string Description { get; }
        public double Priority { get; }

        internal AverageRatingTrackerFact(string factName, string description, double priority, double averageRating)
        {
            FactName = factName;
            Description = description;
            Priority = priority;
            AverageRating = averageRating;
        }
    }
}