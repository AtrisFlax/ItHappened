namespace ItHappened.Domain.Statistics
{
    public class SumScaleTrackerFact : ISingleTrackerFact
    {
        public double SumValue { get; }
        public string MeasurementUnit { get; }
        public string FactName { get; }
        public string Description { get; }
        public double Priority { get; }

        internal SumScaleTrackerFact(string factName, string description, double priority, double sumValue,
            string measurementUnit)
        {
            FactName = factName;
            Description = description;
            Priority = priority;
            SumValue = sumValue;
            MeasurementUnit = measurementUnit;
        }
    }
}