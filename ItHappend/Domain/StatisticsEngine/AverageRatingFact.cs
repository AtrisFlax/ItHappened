using System;
using System.Linq;
using Optional;
using Optional.Unsafe;
using Status = ItHappend.Domain.StatisticServiceStatusCodes;

namespace ItHappend.Domain
{
    public class AverageRatingFact : IAverageRatingFact
    {
        public string Type { get; private set; }
        public string Description { get; private set; }

        public double Priority { get; private set; }
        public double AverageRating { get; private set; }

        public Option<AverageRatingFact, Status> CreateAverageRatingFact(EventTracker eventTracker) =>
            ApplicabilityFunction(eventTracker);

        private static Option<AverageRatingFact, Status> ApplicabilityFunction(EventTracker eventTracker)
        {
            if (eventTracker == null) throw new NullReferenceException();
            if (!eventTracker.HasRating)
            {
                return Option.None<AverageRatingFact, Status>(StatisticServiceStatusCodes.EventTrackerHasNoRating);
            }

            if (eventTracker.Events.Count <= 1)
            {
                return Option.None<AverageRatingFact, Status>(
                    StatisticServiceStatusCodes.EventTrackerHasNotEnoughEvents);
            }

            var collectionWithNotNullRating = eventTracker.Events.Where(@event => @event.Rating.HasValue).ToList();
            if (collectionWithNotNullRating.Count != eventTracker.Events.Count)
            {
                return Option.None<AverageRatingFact, Status>(StatisticServiceStatusCodes
                    .NotAllEventInTrackerHasRating);
            }

            var averageRating = collectionWithNotNullRating.Average(x => x.Rating.ValueOrFailure());


            return Option.Some<AverageRatingFact, Status>(
                new AverageRatingFact
                {
                    Type = nameof(AverageRatingFact),
                    Description = $"Средний рейтинг для события {eventTracker.Events} равен {averageRating}",
                    Priority = Math.Sqrt(averageRating),
                    AverageRating = averageRating
                });
        }
    }
}