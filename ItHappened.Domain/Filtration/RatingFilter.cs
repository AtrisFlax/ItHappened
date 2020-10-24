using System.Collections.Generic;
using System.Linq;

namespace ItHappened.Domain
{
    public class RatingFilter : IEventsFilter
    {
        public string Name { get; }
        public double LowerLimit { get; }
        public double UpperLimit { get; }

        public RatingFilter(string name, double lowerLimit, double upperLimit)
        {
            Name = name;
            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;
        }

        public IReadOnlyCollection<Event> Filter(IReadOnlyCollection<Event> events)
        {
            return events
                .Where(@event => @event.CustomizationsParameters.Rating >= LowerLimit &&
                                 @event.CustomizationsParameters.Rating <= UpperLimit).ToList();
        }
    }
}