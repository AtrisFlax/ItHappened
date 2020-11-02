using System;
using ItHappened.Infrastructure.Mappers;

namespace ItHappened.Infrastructure.Dto
{
    public class LongestBreakTrackerFactDto : FactDto
    {
        public int DurationInDays { get; set; }
        public DateTimeOffset LastEventBeforeBreakDate { get; set; }
        public DateTimeOffset FirstEventAfterBreakDate { get; set; }
    }
}