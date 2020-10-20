using ItHappened.Domain.Statistics;
using LanguageExt;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public static class FromIFactToFactConvertorExtensions
    {
        public static Option<T> ConvertTo<T>(this Option<IStatisticsFact> fact)
        {
            return fact.Map(f => (T) f);
        }
    }
}