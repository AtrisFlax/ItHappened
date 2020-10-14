using System;
using System.Linq;

namespace ItHappend.Domain
{
    public class MostFrequentEvent : FactStatistics//: IFactStatistics<MostFrequentEvent>
    {
        private MostFrequentEvent(string trackingName, int eventsPeriod, string description, double priority) 
            : base(description, priority)
        {
            TrackingName = trackingName;
            EventsPeriod = eventsPeriod;
        }

        private MostFrequentEvent(): base(default, default)
        {
        }

        public string TrackingName { get; }
        public int EventsPeriod { get; }

        public override FactStatistics ApplicabilityFunction(EventTracker[] eventTrackers)
        {
            if (eventTrackers == null) 
                throw new ArgumentNullException(nameof(Event));
            if (eventTrackers.Length < 2)
                throw new ApplicationException("Количество отслеживаний должно быть больше 1");
            if (!(eventTrackers.Count(e => e.Events.Count > 3) > 2))
                throw new ApplicationException(@"Необходимо хотя бы два отслеживания, 
                                в которых больше трех событий!");

            throw new NotImplementedException();
        }

        public static FactStatistics CreateFactStatistics(EventTracker[] eventTrackers) => 
            new MostFrequentEvent().ApplicabilityFunction(eventTrackers);

    }
}