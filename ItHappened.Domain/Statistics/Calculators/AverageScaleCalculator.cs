using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Serilog;

namespace ItHappened.Domain.Statistics
{
    public class AverageScaleCalculator : ISingleTrackerStatisticsCalculator
    {
        public Option<ISingleTrackerTrackerFact> Calculate(IReadOnlyCollection<Event> events, EventTracker tracker)
        {
            if (!CanCalculate(events))
            {
                return Option<ISingleTrackerTrackerFact>.None;
            }

            var averageValue = events.Select(x => x.CustomizationsParameters.Scale).Somes().Average();
            var measurementUnit =
                tracker.CustomizationSettings.ScaleMeasurementUnit.Match(x => x, () => string.Empty);
            
            const string factName = "Среднее значение шкалы";
            var description = $"Сумма значений {measurementUnit} для события {tracker.Name} равно {averageValue}";
            const double priority = 3.0;
            
            return Option<ISingleTrackerTrackerFact>.Some(new AverageScaleTrackerFact(
                factName,
                description,
                priority,
                averageValue,
                measurementUnit
            ));
        }

        private static bool CanCalculate(IReadOnlyCollection<Event> events)
        {
            if (events.Any(@event => @event.CustomizationsParameters.Scale == Option<double>.None))
            {
                return false;
            }

            return events.Count > 1;
        }
    }
}