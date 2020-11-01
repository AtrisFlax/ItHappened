using ItHappened.Infrastructure.Mappers;

namespace ItHappened.Infrastructure.Dto
{
    public class SpecificDayTimeEventFactDto : FactDto
    {
        public double Percentage { get; }
        public string TimeOfTheDay { get; }
    }
}