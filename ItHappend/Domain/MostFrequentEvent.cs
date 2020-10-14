using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Net.Mime;

namespace ItHappend.Domain
{
    public class MostFrequentEvent : IFactStatistics
    {
        private MostFrequentEvent()
        {
        }

        private MostFrequentEvent(string trackingName, double eventsPeriod)
        {
            EventsPeriod = eventsPeriod;
            TrackingName = trackingName;
            Description = $"Чаще всего у вас происходит событие {TrackingName} - раз в {EventsPeriod} дней";
            Priority = 10 / eventsPeriod;
            //Здесь же рисуем изображение
        }

        public string Description { get; }
        public double Priority { get; }
        public string TrackingName { get; }
        public double EventsPeriod { get; }

        //public  Ilustration { get; }

        public IFactStatistics ApplicabilityFunction(EventTracker[] eventTrackers)
        {
            VerifyInputParameters(eventTrackers);
            
            var trackingNameWithMinEventsPeriod = eventTrackers
                .Select(eventTracker =>
                    new
                    {
                        trackingName = eventTracker.Name,
                        eventsPeriod = 1.0 * eventTracker
                            .Events
                            .GroupBy(t => t.HappensDate.Date)
                            .Select(t => t.Key)
                            .Count() / eventTrackers.Length
                    })
                .OrderBy(e => e.eventsPeriod)
                .FirstOrDefault();
            
            return new MostFrequentEvent
                (trackingNameWithMinEventsPeriod.trackingName, trackingNameWithMinEventsPeriod.eventsPeriod);
        }

        public static IFactStatistics CreateFactStatistics(EventTracker[] eventTrackers) => 
            new MostFrequentEvent().ApplicabilityFunction(eventTrackers);

        private void VerifyInputParameters(EventTracker[] eventTrackers)
        {
            if (eventTrackers == null) 
                throw new ArgumentNullException(nameof(Event));
            if (eventTrackers.Length < 2)
                throw new ApplicationException("Количество отслеживаний должно быть больше 1");
            if (!(eventTrackers.Count(e => e.Events.Count > 3) > 2))
                throw new ApplicationException(@"Необходимо хотя бы два отслеживания, 
                                в которых больше трех событий!");
        }

    }
}