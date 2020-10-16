using System;
using System.Linq;
using ItHappend.Domain.Statistics.StatisticsFacts;
using ItHappened.Domain.EventTracker;
using ItHappened.Domain.Statistics.Calculators.ForSingleTracker;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace ItHappend.Domain.Statistics.SingleTrackerCalculator
{
    public class SumScaleCalculator : ISingleTrackerStatisticsCalculator<SumScaleFact>
    {
        public Option<SumScaleFact> Calculate(EventTracker eventTracker)
        {
            if (!CanCalculate(eventTracker)) return Option<SumScaleFact>.None;
            var sumScale = eventTracker.Events.Sum(x => x.Scale.ValueUnsafe());
            var measurementUnit = eventTracker.ScaleMeasurementUnit.ValueUnsafe();
            return Option<SumScaleFact>.Some(new SumScaleFact(
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