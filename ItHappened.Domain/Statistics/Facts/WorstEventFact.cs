using System;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;

namespace ItHappend.Domain.Statistics
{
    public class WorstEventFact : ISingleTrackerFact
    {
        public double Rating { get; }
        public DateTimeOffset HappensDate { get; }
        public Comment Comment { get; }
        public Event EventReference { get; }
        public string FactName { get; }
        public string Description { get; }
        public double Priority { get; }
        
        internal WorstEventFact(string factName,
            string description,
            double priority,
            double rating,
            DateTimeOffset happensDate,
            Comment comment,
            Event eventReference)
        {
            FactName = factName;
            Description = description;
            Priority = priority;
            Rating = rating;
            HappensDate = happensDate;
            Comment = comment;
            EventReference = eventReference;
        }
    }
}