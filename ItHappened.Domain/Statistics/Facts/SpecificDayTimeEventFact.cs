namespace ItHappened.Domain.Statistics
{
    public class SpecificDayTimeFact : ISingleTrackerTrackerFact
    {
        public string FactName { get; }
        public string Description { get; }
        public double Priority { get; }
        public double Percentage { get; }
        public string TimeOfTheDay { get; }

        internal SpecificDayTimeFact(string factName, string description, double priority, double percentage,
            string timeOfTheDay)
        {
            FactName = factName;
            Description = description;
            Priority = priority;
            Percentage = percentage;
            TimeOfTheDay = timeOfTheDay;
        }
    }
}