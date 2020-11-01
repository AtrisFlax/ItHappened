using System;
using ItHappened.Infrastructure.Mappers;

namespace ItHappened.Infrastructure.Dto
{
    public class MostEventfulDayTrackersFactDto  : FactDto
    {
        public DateTimeOffset DayWithLargestEventsCount { get; }
        public int EventsCount { get; }
    }
}