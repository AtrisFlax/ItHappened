namespace ItHappened.Domain.Statistics
{
    public class LongestBreakFact : ISingleTrackerStatisticsFact
    {
        public string FactName { get; }
        public string Description { get; }
        public double Priority { get; }
        public int DurationInDays { get; }
        public Event LastEventBeforeBreakDate { get; }
        public Event FirstEventAfterBreakDate { get; }

        internal LongestBreakFact(string factName,
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