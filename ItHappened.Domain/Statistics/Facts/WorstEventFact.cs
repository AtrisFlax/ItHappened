using System;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using LanguageExt;

namespace ItHappend.Domain.Statistics
{
    public class WorstEventFact : ISingleTrackerStatisticsFact
    {
        public string FactName { get; }
        public string Description { get; }
        public double Priority { get; }
        public double Rating { get; }
        public DateTimeOffset HappensDate { get; }
        public Option<Comment> Comment { get; }
        public Event EventReference { get; }
        
        internal WorstEventFact(string factName, 
            string description, 
            double priority, 
            double rating,
            DateTimeOffset happensDate, 
            Option<Comment> comment, 
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