using System;
using System.Collections.Generic;
using ItHappened.Infrastructure.Mappers;

namespace ItHappened.Infrastructure.Dto
{
    public class OccursOnCertainDaysOfTheWeekTrackerFactDto : FactDto
    {
        public IEnumerable<DayOfWeek> DaysOfTheWeek { get; }
        public double Percentage { get; }
    }
}