using System;
using ItHappend.Domain.EventCustomization;

namespace ItHappend.Domain.Statistics.StatisticsFacts
{
    public class WorstEvent : ISingleTrackerStatisticsFact
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