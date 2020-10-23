using ItHappened.Domain.Statistics;
using LanguageExt;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public static class FromIFactToFactConverterExtensions
    {
        public static Option<T> ConvertTo<T>(this Option<ISingleTrackerTrackerFact> fact)
        {
            return fact.Map(f => (T) f);
        }
        
        public static Option<T> ConvertTo<T>(this Option<IMultipleTrackerTrackerFact> fact)
        {
            return fact.Map(f => (T) f);
        }
    }
}