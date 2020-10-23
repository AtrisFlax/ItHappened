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
            if (!CanCalculate(events, tracker)) return Option<ISingleTrackerTrackerFact>.None;
            var averageValue = events.Select(x => x.CustomizationsParameters.Scale).Somes().Average();
            var measurementUnit = tracker.CustomizationSettings.ScaleMeasurementUnit
                .Match(x => x,
                    () => "No Scale");
            return Option<ISingleTrackerTrackerFact>.Some(new AverageScaleTrackerFact(
                "Среднее значение шкалы",
                $"Сумма значений {measurementUnit} для события {tracker.Name} равно {averageValue}",
                3.0,
                averageValue,
                measurementUnit
            ));
        }

        private static bool CanCalculate(IReadOnlyCollection<Event> events, EventTracker tracker)
        {
            if (tracker.CustomizationSettings.ScaleMeasurementUnit.IsNone)
            {
                return false;
            }

            if (events.Any(@event => @event.CustomizationsParameters.Scale == Option<double>.None))
            {
                return false;
            }

            return events.Count > 1;
        }
    }
}