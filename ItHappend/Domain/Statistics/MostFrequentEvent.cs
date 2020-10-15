using System;
using System.Linq;
using Status = ItHappend.StatisticService.StatisticServiceStatusCodes;

namespace ItHappend.Domain.Statistics
{
    public class MostFrequentEvent
    {
        public string Type { get; }
        public string Description { get; }

        public double Priority { get; }
        public string TrackingName { get; }
        public int EventsPeriod { get; }

        public static (MostFrequentEvent, Status) CreateFactStatistics(EventTracker[] eventTrackers) =>
            new MostFrequentEvent().ApplicabilityFunction(eventTrackers);

        private (MostFrequentEvent, Status) ApplicabilityFunction(EventTracker[] eventTrackers)
        {
            if (eventTrackers == null)
                throw new NullReferenceException();
            if (eventTrackers.Length < 2)
                return (null, Status.ApplicabilityFunctionDoesNotCompute);
            if (!(eventTrackers.Count(e => e.Events.Count > 3) > 2))
                return (null, Status.ApplicabilityFunctionDoesNotCompute);


            //TODO calculation Description, Priority, TrackingName, EventsPeriod

            //TODO Type = nameof(MostFrequentEvent)
            return (new MostFrequentEvent( /*TODO  pass arguments for constructor */), Status.Ok);
        }
    }
}