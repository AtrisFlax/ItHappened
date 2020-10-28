using System.Collections.Generic;
using System.Linq;

namespace ItHappened.Domain
{
    public class ScaleFilter : IEventsFilter
    {
        public string Name { get; }
        public double LowerLimit { get; }
        public double UpperLimit { get; }

        public ScaleFilter(string name, double lowerLimit, double upperLimit)
        {
            Name = name;
            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;
        }

        public IEnumerable<Event> Filter(IEnumerable<Event> events)
        {
            return events
                .Where(@event => @event.CustomizationsParameters.Scale >= LowerLimit &&
                                 @event.CustomizationsParameters.Scale <= UpperLimit);
        }
    }
}