using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public interface ISpecificCalculator
    {
        Option<ISpecificFact> Calculate(EventTracker eventTracker);
    }
}