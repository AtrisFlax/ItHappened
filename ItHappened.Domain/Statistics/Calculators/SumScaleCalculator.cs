using System.Collections.Generic;
using System.Linq;
using ItHappend.Domain.Statistics;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using Serilog;

namespace ItHappened.Domain.Statistics
{
    public class SumScaleCalculator : ISingleTrackerStatisticsCalculator
    {
        private readonly IEventRepository _eventRepository;

        public SumScaleCalculator(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        public Option<ISingleTrackerFact> Calculate(EventTracker eventTracker)
        {
            var loadAllTrackerEvents = _eventRepository.LoadAllTrackerEvents(eventTracker.Id);
            if (!CanCalculate(eventTracker, loadAllTrackerEvents))
            {
                return Option<ISingleTrackerFact>.None;
            }
            var sumScale = loadAllTrackerEvents.Select(x=>x.CustomizationsParameters.Scale).Somes().Sum();
            var measurementUnit = eventTracker.Customizations.ScaleMeasurementUnit.Match(
                x=>x,
                ()=>
                {
                    Log.Error("EventTracker has not scale while calculate inside SumScaleCalculator");
                    return "";
                } );
            return Option<ISingleTrackerFact>.Some(new SumScaleFact(
                "Суммарное значение шкалы",
                $"Сумма значений {measurementUnit} для события {eventTracker.Name} равна {sumScale}",
                2.0,
                sumScale,
                measurementUnit
            ));
        }

        private static bool CanCalculate(EventTracker eventTracker, IReadOnlyList<Event> loadAllTrackerEvents)
        {
            if (eventTracker.Customizations.ScaleMeasurementUnit.IsNone)
            {
                return false;
            }

            if (loadAllTrackerEvents.Any(@event => @event.CustomizationsParameters.Scale == Option<double>.None))
            {
                return false;
            }
            return loadAllTrackerEvents.Count > 1;
        }
    }
}