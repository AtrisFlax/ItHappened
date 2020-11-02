using System;
using System.Collections.Generic;
using ItHappened.Infrastructure.Mappers;

namespace ItHappened.Infrastructure.Dto
{
    public class OccursOnCertainDaysOfTheWeekTrackerFactDto : FactDto
    {
        public string DaysOfTheWeek { get; set; }
        public double Percentage { get; set; }
    }
}