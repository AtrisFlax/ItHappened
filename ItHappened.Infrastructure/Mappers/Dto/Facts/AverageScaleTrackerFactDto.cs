namespace ItHappened.Infrastructure.Mappers
{
    public class AverageScaleTrackerFactDto : FactDto
    {
        public double AverageValue { get; set; }
        public string MeasurementUnit { get; set; }
    }
}