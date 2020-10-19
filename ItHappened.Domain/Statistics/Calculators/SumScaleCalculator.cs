using System;
using System.Linq;
using ItHappend.Domain.Statistics;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using Serilog;

namespace ItHappened.Domain.Statistics
{
    public class SumScaleCalculator : ISingleTrackerStatisticsCalculator
    {
        public Option<IStatisticsFact> Calculate(EventTracker eventTracker)
        {
            if (!CanCalculate(eventTracker)) return Option<IStatisticsFact>.None;
            var sumScale = eventTracker.Events.Sum(x =>
            {
                return x.Scale.Match(
                    r => r,
                    () =>
                    {
                        Log.Error("Empty Scale while calculating SumScaleCalculator ");
                        return 0;
                    });
            });
            var measurementUnit = eventTracker.ScaleMeasurementUnit.Match(
                x => x,
                () =>
                {
                    Log.Error("Empty Scale while calculating SumScaleCalculator ");
                    return string.Empty;
                }
            );
            return Option<IStatisticsFact>.Some(new SumScaleFact(
                "Суммарное значение шкалы",
                $"Сумма значений {measurementUnit} для события {eventTracker.Name} равна {sumScale}",
                2.0,
                sumScale,
                measurementUnit
            ));
        }


        private bool CanCalculate(EventTracker eventTracker)
        {
            if (eventTracker.ScaleMeasurementUnit.IsNone)
            {
                return false;
            }

            if (eventTracker.Events.Any(@event => @event.Scale == Option<double>.None))
            {
                return false;
            }

            return eventTracker.Events.Count > 1;
        }
    }
}