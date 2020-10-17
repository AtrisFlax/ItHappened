using System;
using ItHappened.Domain;
using ItHappened.Domain.EventCustomization;
using ItHappened.Domain.Statistics.Facts;
using LanguageExt;

namespace ItHappend.Domain.Statistics.StatisticsFacts
{
    public class WorstEventFact : IStatisticsFact
    {
        public string FactName { get; }
        public string Description { get; }
        public double Priority { get; }
        public DateTimeOffset Date { get; }
        public Option<Comment> Comment { get; }
        public Event EventReference { get; }
        
        public WorstEventFact(string factName, string description, double priority, DateTimeOffset date, Option<Comment> comment, Event eventReference)
        {
            FactName = factName;
            Description = description;
            Priority = priority;
            Date = date;
            Comment = comment;
            EventReference = eventReference;
        }
    }
}