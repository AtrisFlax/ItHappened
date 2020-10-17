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
        public Comment Comment { get; }
        public Event EventReference { get; }
        
        public WorstEvent(string description, double priority, DateTimeOffset date, Comment comment, Event eventReference)
        {
            Description = description;
            Priority = priority;
            Date = date;
            Comment = comment;
            EventReference = eventReference;
        }
    }
}