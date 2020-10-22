using System;
using System.Collections.Generic;
using System.Linq;

namespace ItHappened.Domain.Statistics
{
    /*public class SingleTrackerTimeOfDayEventFact : ISingleTrackerFact
    {
        public double Percentage { get; }
        public string TimeOfTheDay { get; }
        public IReadOnlyCollection<(string Title, string TimeOfTheDay, double Percentage)> VisualizationData { get; }
        public string FactName { get; } = nameof(SingleTrackerTimeOfDayEventFact);
        public string Description { get; }
        public double Priority { get; }

        internal SingleTrackerTimeOfDayEventFact(double percentage, string timeOfTheDay,
            IEnumerable<(string timeOfTheDay, double percentage)> visualizationData)
        {
            Percentage = Math.Round(percentage);
            TimeOfTheDay = timeOfTheDay;
            Description = $"В {Percentage}% случаев событие \"{"*"}\" происходит {timeOfTheDay}";
            Priority = 0.14 * percentage;
            VisualizationData = visualizationData.ToList();
        }
    }*/
}