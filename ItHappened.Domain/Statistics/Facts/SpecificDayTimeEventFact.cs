using System;
using System.Collections.Generic;
using System.Linq;

namespace ItHappened.Domain.Statistics
{
    public class SpecificTimeOfDayEventFact : IStatisticsFact
    {
        internal SpecificTimeOfDayEventFact
        (double percentage, string eventName, string timeOfTheDay,
            IEnumerable<(string title, string timeOfTheDay, double percentage)> visualizationData)
        {
            Percentage = Math.Round(percentage);
            TimeOfTheDay = timeOfTheDay;
            Description = $"В {Percentage}% случаев событие \"{eventName}\" происходит {timeOfTheDay}";
            Priority = 0.14 * percentage;
            VisualizationData = visualizationData.ToList();
        }

        public double Percentage { get; }
        public string TimeOfTheDay { get; }
        public IReadOnlyCollection<(string Title, string TimeOfTheDay, double Percentage)> VisualizationData { get; }

        public string FactName { get; } = nameof(SpecificTimeOfDayEventFact);
        public string Description { get; }
        public double Priority { get; }
    }
}