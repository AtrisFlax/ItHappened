using System;
using ItHappened.Infrastructure.Mappers;

namespace ItHappened.Infrastructure.Dto
{
    public class MostEventfulWeekTrackersFactDto : FactDto
    {
        public DateTimeOffset WeekWithLargestEventCountFirstDay { get; set; }
        public DateTimeOffset WeekWithLargestEventCountLastDay { get; set; }
        public int EventsCount { get; set; }
    }
}