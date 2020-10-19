using System;
using System.Linq;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using Serilog;

namespace ItHappened.Domain.Statistics
{
    public class AverageRatingCalculator : ISingleTrackerStatisticsCalculator
    {
        private readonly IEventRepository _eventRepository;
        public AverageRatingCalculator(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        public Option<IStatisticsFact> Calculate(EventTracker eventTracker)
        {
            if (!CanCalculate(eventTracker)) return Option<IStatisticsFact>.None;

            var averageRating = _eventRepository.LoadAllTrackerEvents(eventTracker.Id).Average(x =>
            {
                return x.Rating.Match(
                    r => r,
                    () =>
                    {
                        Log.Warning("Calculate Empty Rating While Calculate AverageRatingCalculator");
                        return 0;
                    });
            });
            return Option<IStatisticsFact>.Some(new AverageRatingFact(
                "Среднее значение оценки",
                $"Средний рейтинг для события {eventTracker.Name} равен {averageRating}",
                Math.Sqrt(averageRating),
                averageRating
            ));
        }

        private bool CanCalculate(EventTracker eventTracker)
        {
            if (!eventTracker.HasRating) return false;
            var trackerEvents=_eventRepository.LoadAllTrackerEvents(eventTracker.Id);
            if (trackerEvents.Any(@event => @event.Rating == Option<double>.None)) return false;

            return trackerEvents.Count > 1;
        }
    }
}