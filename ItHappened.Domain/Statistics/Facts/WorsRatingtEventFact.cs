using System;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public class WorstRatingEventFact : ISingleTrackerFact
    {
        public double WorstRating { get; }
        public DateTimeOffset WorstEventDate { get; }
        public Option<Comment> WorstEventComment { get; }
        public string FactName { get; }
        public string Description { get; }
        public double Priority { get; }
        public Guid WorstEventId { get; } //Event reference 

        internal WorstRatingEventFact(string factName,
            string description,
            double priority,
            double worstRating,
            DateTimeOffset worstEventDate,
            Option<Comment> worstEventComment, Guid worstEventId)
        {
            FactName = factName;
            Description = description;
            Priority = priority;
            WorstRating = worstRating;
            WorstEventDate = worstEventDate;
            WorstEventComment = worstEventComment;
            WorstEventId = worstEventId;
        }
    }
}