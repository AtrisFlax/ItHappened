using ItHappened.Domain.Statistics;
using LanguageExt;

namespace ItHappened.Infrastructure
{
    public static class FromIFactToFactConverterExtensions
    {
        public static Option<T> ConvertTo<T>(this Option<ISingleTrackerFact> fact)
        {
            return fact.Map(f => (T) f);
        }
        
        public static Option<T> ConvertTo<T>(this Option<IMultipleTrackersFact> fact)
        {
            return fact.Map(f => (T) f);
        }
        
        public static T ConvertTo<T>(this ISingleTrackerFact fact)
        {
            return (T) fact;
        }
        
        public static T ConvertTo<T>(this IMultipleTrackersFact fact)
        {
            return (T) fact;
        }
    }
}