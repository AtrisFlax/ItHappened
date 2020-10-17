using System;
using ItHappend.Domain.EventCustomization;
using LanguageExt;

namespace ItHappend.Domain.Statistics.StatisticsFacts
{
    public class WorstEvent : IStatisticsFact
    {
        public string Description { get; }
        public double Priority { get; }
        public DateTimeOffset Date { get; }
        public Option<Comment> Comment { get; }
        public Event EventReference { get; }
        
        public WorstEvent(string description, double priority, DateTimeOffset date, Option<Comment> comment, Event eventReference)
        {
            Description = description;
            Priority = priority;
            Date = date;
            Comment = comment;
            EventReference = eventReference;
        }
    }
}