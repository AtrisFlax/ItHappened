using System;
using System.Linq;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public class LongestBreakCalculator : ISpecificCalculator
    {
        private readonly IEventRepository _eventRepository;
        
        public LongestBreakCalculator(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        
        public Option<ISpecificFact> Calculate(EventTracker eventTracker)
        {
            if (!CanCalculate(eventTracker)) return Option<ISpecificFact>.None;

            var (lastEventBeforeBreak, firstEventAfterBreak) = GetFirstAndLastEventOfTheLongestBreak(eventTracker);
            var maxDurationInDays = (firstEventAfterBreak.HappensDate - lastEventBeforeBreak.HappensDate).Days;
            var priority = Math.Sqrt(maxDurationInDays);
            var description =
                $"Самый большой перерыв в {eventTracker.Name} произошёл с {lastEventBeforeBreak}" +
                $" до {firstEventAfterBreak}, он занял {maxDurationInDays} дней";
            const string factName = "Самый долгий перерыв";

            return Option<ISpecificFact>.Some(new LongestBreakFact(factName,
                description,
                priority,
                maxDurationInDays,
                lastEventBeforeBreak,
                firstEventAfterBreak));
        }

        private bool CanCalculate(EventTracker eventTracker)
        {
            if (_eventRepository.LoadAllTrackerEvents(eventTracker.Id).Count <= 10)
                return false;

            var (lastEventBeforeBreak, firstEventAfterBreak) = GetFirstAndLastEventOfTheLongestBreak(eventTracker);
            var maxDuration = (firstEventAfterBreak.HappensDate - lastEventBeforeBreak.HappensDate).Days;
            if ((firstEventAfterBreak.HappensDate - DateTimeOffset.Now).Days > 7)
                return false;

            var averageTimeBetweenEvents = GetAverageDurationBetweenEvents(eventTracker);
            return !(maxDuration < averageTimeBetweenEvents * 3);
        }

        private (Event lastEventBeforeBreak, Event firstEventAfterBreak) GetFirstAndLastEventOfTheLongestBreak(
            EventTracker eventTracker)
        {
            var events = _eventRepository.LoadAllTrackerEvents(eventTracker.Id);
            var lastEventBeforeBreak = events[0];
            var firstEventAfterBreak = events[1];
            var maxDuration = TimeSpan.Zero;
            for (var i = 1; i < events.Count - 1; i++)
            {
                var duration = events[i].HappensDate - events[i + 1].HappensDate;
                if (duration > maxDuration)
                {
                    maxDuration = duration;
                    lastEventBeforeBreak = events[i];
                    firstEventAfterBreak = events[i + 1];
                }
            }

            return (lastEventBeforeBreak, firstEventAfterBreak);
        }

        private double GetAverageDurationBetweenEvents(EventTracker eventTracker)
        {
            var events = _eventRepository.LoadAllTrackerEvents(eventTracker.Id);
            var numberOfBreaks = events.Count - 1;
            return (double)(events.First().HappensDate - events.Last().HappensDate).Days / numberOfBreaks;
        }
    }
}