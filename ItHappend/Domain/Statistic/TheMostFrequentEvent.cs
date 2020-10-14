using System.Runtime;

namespace ItHappend.Domain
{
    public class TheMostFrequentEvent
    {
        public string EventTrackerName { get; }
        public double EventsPeriod { get; }
        public double Priority { get; }
        public string Description { get; }

        public TheMostFrequentEvent(string eventTrackerName, double eventsPeriod, object barChart, double priority)
        {
            EventTrackerName = eventTrackerName;
            EventsPeriod = eventsPeriod;
            Priority = priority;
            Description = $"Чаще всего у вас происходит событие {EventTrackerName} - раз в {EventsPeriod} дней";
        }
    }
}