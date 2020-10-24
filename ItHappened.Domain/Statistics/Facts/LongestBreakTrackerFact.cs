namespace ItHappened.Domain.Statistics
{
    public class LongestBreakTrackerFact : ISingleTrackerFact
    {
        public int DurationInDays { get; }
        public Event LastEventBeforeBreakDate { get; }
        public Event FirstEventAfterBreakDate { get; }
        public string FactName { get; }
        public string Description { get; }
        public double Priority { get; }

        internal LongestBreakTrackerFact(string factName,
            string description,
            double priority,
            int durationInDays, Event lastEventBeforeBreakDate, Event firstEventAfterBreakDate)
        {
            FactName = factName;
            Description = description;
            Priority = priority;
            DurationInDays = durationInDays;
            LastEventBeforeBreakDate = lastEventBeforeBreakDate;
            FirstEventAfterBreakDate = firstEventAfterBreakDate;
        }
    }
}