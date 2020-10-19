using System;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace ItHappend.Domain.Statistics
{
    public class SumScaleCalculator : ISingleTrackerStatisticsCalculator
    {
        public Option<IStatisticsFact> Calculate(EventTracker eventTracker)
        {
            if (!CanCalculate(eventTracker)) return Option<IStatisticsFact>.None;
            var sumScale = eventTracker.Events.Sum(x => x.Scale.ValueUnsafe());
            var measurementUnit = eventTracker.ScaleMeasurementUnit.ValueUnsafe();
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