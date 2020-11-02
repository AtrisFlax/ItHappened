using ItHappened.Infrastructure.Mappers;

namespace ItHappened.Infrastructure.Dto
{
    public class SpecificDayTimeFactDto : FactDto
    {
        public double Percentage { get; set; }
        public string TimeOfTheDay { get; set; }
    }
}