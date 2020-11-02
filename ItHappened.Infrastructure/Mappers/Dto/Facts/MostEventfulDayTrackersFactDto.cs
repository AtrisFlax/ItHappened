using System;
using ItHappened.Infrastructure.Mappers;

namespace ItHappened.Infrastructure.Dto
{
    public class MostEventfulDayTrackersFactDto : FactDto
    {
        public DateTimeOffset DayWithLargestEventsCount { get; set; }
        public int EventsCount { get; set; }
    }
}