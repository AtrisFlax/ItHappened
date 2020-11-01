using System;
using ItHappened.Infrastructure.Mappers;

namespace ItHappened.Infrastructure.Dto
{
    public class MostEventfulWeekTrackersFactDto : FactDto
    {
        public DateTimeOffset WeekWithLargestEventCountFirstDay { get; }
        public DateTimeOffset WeekWithLargestEventCountLastDay { get; }
        public int EventsCount { get; }
    }
}