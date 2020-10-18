namespace ItHappened.Domain.EventCustomization
{
    public class Scale
    {
        public string MeasurementUnitName { get; }
        public string Value { get; }

        public Scale(string measurementUnitName, string value)
        {
            MeasurementUnitName = measurementUnitName;
            Value = value;
        }
    }
}