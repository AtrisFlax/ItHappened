using System;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public class BestRatingEventFact : ISingleTrackerFact
    {
        public string FactName { get; }
        public string Description { get; }
        public double Priority { get; }
        public double BestRating { get; }
        public DateTimeOffset BestEventDate { get; }
        public Option<Comment> BestEventComment { get; }

        internal BestRatingEventFact(string factName, string description, double priority, double bestRating, DateTimeOffset bestEventDate, Option<Comment> bestEventComment)
        {
            FactName = factName;
            Description = description;
            Priority = priority;
            BestRating = bestRating;
            BestEventDate = bestEventDate;
            BestEventComment = bestEventComment;
        }
    }
}