using Optional;

namespace ItHappend.Domain
{
    public interface IAverageRatingFact
    {
        Option<AverageRatingFact, StatisticServiceStatusCodes> CreateAverageRatingFact(EventTracker eventTracker);
    }
}