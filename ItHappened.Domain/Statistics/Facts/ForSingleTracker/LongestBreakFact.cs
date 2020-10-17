using System;

namespace ItHappened.Domain.Statistics.Facts.ForSingleTracker
{
    public class LongestBreakFact : ISingleTrackerStatisticsFact
    {
        public LongestBreakFact(EventTracker targetEventTracker,
            double priority,
            int durationInDays,
            Event lastEventBeforeBreakDate,
            Event firstEventAfterBreakDate)
        {
            TargetEventTracker = targetEventTracker;
            DurationInDays = durationInDays;
            LastEventBeforeBreakDate = lastEventBeforeBreakDate;
            FirstEventAfterBreakDate = firstEventAfterBreakDate;
        }
        
        public string FactName { get; }
        public string Description =>
            $"Самый большой перерыв в {TargetEventTracker.Name} произошёл с {LastEventBeforeBreakDate}" +
            $" до {FirstEventAfterBreakDate}, он занял {DurationInDays} дней";

        public double Priority => Math.Sqrt(DurationInDays);
        public int DurationInDays { get; }
        public Event LastEventBeforeBreakDate { get; }
        public Event FirstEventAfterBreakDate { get; }
        public EventTracker TargetEventTracker { get; }
    }
}