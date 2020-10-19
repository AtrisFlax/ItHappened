using ItHappened.Domain.Statistics;

namespace ItHappend.Domain.Statistics
{
    public class SumScaleFact : IStatisticsFact
    {
        public string FactName { get; }
        public string Description { get; }
        public double Priority { get; }

        public double SumValue { get; }

        public string MeasurementUnit { get; }

        internal SumScaleFact(string factName, string description, double priority, double sumValue, string measurementUnit)
        {
            FactName = factName;
            Description = description;
            Priority = priority;
            SumValue = sumValue;
            MeasurementUnit = measurementUnit;
        }
    }
}