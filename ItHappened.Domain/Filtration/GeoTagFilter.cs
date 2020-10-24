using System.Collections.Generic;
using System.Linq;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Domain
{
    public class GeoTagFilter : IEventsFilter
    {
        public string Name { get; }
        public GeoTag LowerLeftCorner { get; }
        public GeoTag UpperRightCorner { get; }

        public GeoTagFilter(string name, GeoTag lowerLeftCorner, GeoTag upperRightCorner)
        {
            Name = name;
            LowerLeftCorner = lowerLeftCorner;
            UpperRightCorner = upperRightCorner;
        }

        public IEnumerable<Event> Filter(IReadOnlyCollection<Event> events)
        {
            if (LowerLeftCorner.GpsLat > UpperRightCorner.GpsLat || LowerLeftCorner.GpsLng > UpperRightCorner.GpsLng)
            {
                return new List<Event>();
            }

            return events.Where(@event => @event.CustomizationsParameters.GeoTag.IsSome)
                .Where(eventItem =>
                    eventItem.CustomizationsParameters.GeoTag.ValueUnsafe().GpsLat >= LowerLeftCorner.GpsLat &&
                    eventItem.CustomizationsParameters.GeoTag.ValueUnsafe().GpsLat <= UpperRightCorner.GpsLat &&
                    eventItem.CustomizationsParameters.GeoTag.ValueUnsafe().GpsLng >= LowerLeftCorner.GpsLng &&
                    eventItem.CustomizationsParameters.GeoTag.ValueUnsafe().GpsLng <= UpperRightCorner.GpsLng).ToList();
        }
    }
}