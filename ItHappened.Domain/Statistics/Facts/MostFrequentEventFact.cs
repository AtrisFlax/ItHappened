using System;
using System.Collections.Generic;
using System.Linq;

namespace ItHappened.Domain.Statistics
{
    public class MostFrequentEventFact : IStatisticsFact
    {
        protected internal MostFrequentEventFact(
            string trackingName, double eventsPeriod, IEnumerable<(string, double)> eventTrackersWithPeriods)
        {
            TrackingName = trackingName;
            EventsPeriod = Math.Round(eventsPeriod, 2);
            Description = $"Чаще всего у вас происходит событие \"{TrackingName}\" - раз в {EventsPeriod} дней";
            Priority = 10 / EventsPeriod;
            EventTrackersWithPeriods = eventTrackersWithPeriods.ToList();
        }

        public string TrackingName { get; }
        public double EventsPeriod { get; }
        public IReadOnlyCollection<(string TrackingName, double EventPeriod)> EventTrackersWithPeriods { get; }

        public string FactName { get; } = nameof(MostFrequentEventFact);
        public string Description { get; }
        public double Priority { get; }
    }
}