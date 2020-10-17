using System.Collections.Generic;
using System.Linq;

namespace ItHappened.Domain.Statistics.Facts.ForSingleTracker
{
    public class SpecificTimeOfDayEventFact : ISingleTrackerStatisticsFact
    {
        protected internal SpecificTimeOfDayEventFact
            (double percentage, string eventName, string timeOfTheDay, 
            IEnumerable<(string title, string timeOfTheDay, double percentage)> visualizationData)
        {
            Percentage = percentage;
            TimeOfTheDay = timeOfTheDay;
            Description = $"В {percentage}% случаев событие \"{eventName}\" происходит {timeOfTheDay}";
            Priority = 0.14 * percentage;
            VisualizationData = visualizationData.ToList();
        }

        public string FactName { get; } = nameof(SpecificTimeOfDayEventFact);
        public string Description { get; }
        public double Priority { get; }
        public double Percentage { get; }
        public string TimeOfTheDay { get; }
        public List<(string Title, string TimeOfTheDay, double Percentage)> VisualizationData { get; }
    }
    
}