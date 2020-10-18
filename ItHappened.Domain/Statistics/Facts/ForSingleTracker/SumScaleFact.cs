using ItHappened.Domain.Statistics.Facts.ForSingleTracker;

namespace ItHappend.Domain.Statistics.StatisticsFacts
{
    public class SumScaleFact : ISingleTrackerStatisticsFact
    {
        public string Type { get; }
        public string Description { get; }
        public double Priority { get; }

        public double SumValue { get; }

        public string MeasurementUnit { get; }

        public SumScaleFact(string type, string description, double priority, double sumValue, string measurementUnit)
        {
            Type = type;
            Description = description;
            Priority = priority;
            SumValue = sumValue;
            MeasurementUnit = measurementUnit;
        }
    }
}