using ItHappened.Domain.Statistics;
using LanguageExt;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public static class FromIFactToFactConvertorExtensions
    {
        public static Option<T> ConvertTo<T>(this Option<ISpecificFact> fact)
        {
            return fact.Map(f => (T) f);
        }
        
        public static Option<T> ConvertTo<T>(this Option<IGeneralFact> fact)
        {
            return fact.Map(f => (T) f);
        }
    }
}