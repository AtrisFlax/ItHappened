using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using Serilog;

namespace ItHappened.Domain.Statistics
{
    public class SumScaleCalculator : ISingleTrackerStatisticsCalculator
    {
        public Option<ISingleTrackerTrackerFact> Calculate(IReadOnlyCollection<Event> events, EventTracker tracker)
        {
            if (!CanCalculate(events))
            {
                return Option<ISingleTrackerTrackerFact>.None;
            }

            var sumScale = events.Select(x => x.CustomizationsParameters.Scale).Somes().Sum();
            var measurementUnit = tracker.CustomizationSettings.ScaleMeasurementUnit
                .Match(x => x,
                    () => "No Scale"
                );
            return Option<ISingleTrackerTrackerFact>.Some(new SumScaleTrackerFact(
                "Суммарное значение шкалы",
                $"Сумма значений {measurementUnit} для события {tracker.Name} равна {sumScale}",
                2.0,
                sumScale,
                measurementUnit
            ));
        }

        private static bool CanCalculate(IReadOnlyCollection<Event> loadAllTrackerEvents)
        {
            if (loadAllTrackerEvents.Any(@event => @event.CustomizationsParameters.Scale == Option<double>.None))
            {
                return false;
            }

            return loadAllTrackerEvents.Count > 1;
        }
    }
}