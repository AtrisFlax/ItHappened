using ItHappened.Domain.Statistics.Facts.ForSingleTracker;

namespace ItHappend.Domain.Statistics.StatisticsFacts
{
    public class AverageRatingFact : ISingleTrackerStatisticsFact
    {
        public string Type { get; }
        public string Description { get; }
        public double Priority { get; }
        public double AverageRating { get; }

        public AverageRatingFact(string type, string description, double priority, double averageRating)
        {
            Type = type;
            Description = description;
            Priority = priority;
            AverageRating = averageRating;
        }
    }
}