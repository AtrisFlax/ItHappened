using ItHappened.Infrastructure.Mappers;

namespace ItHappened.Infrastructure.Dto
{
    public class SumScaleTrackerFactDto : FactDto
    {
        public double SumValue { get; set;}
        public string MeasurementUnit { get; set;}
    }
}