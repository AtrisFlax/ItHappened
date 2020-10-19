namespace ItHappened.Domain.Statistics
{
    public class AverageRatingFact : IStatisticsFact
    {
        public string FactName { get; }
        public string Description { get; }
        public double Priority { get; }
        public double AverageRating { get; }

        internal AverageRatingFact(string type, string description, double priority, double averageRating)
        {
            FactName = type;
            Description = description;
            Priority = priority;
            AverageRating = averageRating;
        }
    }
}