using System;

namespace ItHappened.Domain.Statistics
{
    public class BestEventFact : IStatisticsFact
    {
        public DateTimeOffset HappensDate { get; }
        public Comment Comment { get; }
        public Event EventReference { get; }
        public string FactName { get; }
        public string Description { get; }
        public double Priority { get; }
        
        internal BestEventFact(string factName, string description, double priority, double rating,
            DateTimeOffset happensDate, Comment comment, Event eventReference)
        {
            FactName = factName;
            Description = description;
            Priority = priority;
            HappensDate = happensDate;
            Comment = comment;
            EventReference = eventReference;
        }
    }
}