using System;
using System.Linq;
using LanguageExt;
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
        
        public Option<ISingleTrackerFact> Calculate(EventTracker eventTracker)
        {
            if (!CanCalculate(eventTracker)) return Option<ISingleTrackerFact>.None;

            var averageRating = _eventRepository.LoadAllTrackerEvents(eventTracker.Id).Average(x =>
            {
                return x.CustomizationsParameters.Rating.Match(
                    r => r,
                    () =>
                    {
                        Log.Warning("Calculate Empty Rating While Calculate AverageRatingCalculator");
                        return 0;
                    });
            });
            return Option<ISingleTrackerFact>.Some(new AverageRatingFact(
                "Среднее значение оценки",
                $"Средний рейтинг для события {eventTracker.Name} равен {averageRating}",
                Math.Sqrt(averageRating),
                averageRating
            ));
        }

        private bool CanCalculate(EventTracker eventTracker)
        {
            if (!eventTracker.CustomizationsSettings.RatingIsOptional) return false;
            var trackerEvents=_eventRepository.LoadAllTrackerEvents(eventTracker.Id);
            if (trackerEvents.Any(@event => @event.CustomizationsParameters.Rating == Option<double>.None)) return false;
            return trackerEvents.Count > 1;
        }
    }
}