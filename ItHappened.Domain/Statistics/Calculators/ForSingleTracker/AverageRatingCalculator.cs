using System;
using System.Linq;
using ItHappened.Domain.Statistics.Facts.ForSingleTracker;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Domain.Statistics.Calculators.ForSingleTracker
{
    public class AverageRatingCalculator : ISingleTrackerStatisticsCalculator<AverageRatingFact>
    {
        public Option<AverageRatingFact> Calculate(EventTracker eventTracker)
        {
            if (!CanCalculate(eventTracker)) return Option<AverageRatingFact>.None;
            var averageRating = eventTracker.Events.Average(x => x.Rating.ValueUnsafe());
            var description = $"Средний рейтинг для события {eventTracker.Name} равен {averageRating}";
            var priority = Math.Sqrt(averageRating);
            
            return Option<AverageRatingFact>.Some(new AverageRatingFact(
                eventTracker,
                description,
                priority,
                averageRating
            ));
        }

        private bool CanCalculate(EventTracker eventTracker)
        {
            if (!eventTracker.HasRating)
            {
                return false;
            }
            return eventTracker.Events.Count > 1;
        }
    }
}