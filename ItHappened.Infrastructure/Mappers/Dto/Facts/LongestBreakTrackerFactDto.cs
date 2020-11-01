using System;
using ItHappened.Infrastructure.Mappers;

namespace ItHappened.Infrastructure.Dto
{
    public class LongestBreakTrackerFactDto : FactDto
    {
        public int DurationInDays { get; }
        public DateTimeOffset LastEventBeforeBreakDate { get; }
        public DateTimeOffset FirstEventAfterBreakDate { get; }
    }
}