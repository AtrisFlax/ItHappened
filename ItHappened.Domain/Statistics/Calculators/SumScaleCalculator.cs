using System.Collections.Generic;
using System.Linq;
using ItHappend.Domain.Statistics;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using Serilog;

namespace ItHappened.Domain.Statistics
{
    public class SumScaleCalculator : ISpecificCalculator
    {
        private const double PriorityValue = 2.0;
        private const int EventsThreshold = 1;
        private readonly IEventRepository _eventRepository;

        public SumScaleCalculator(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        public Option<ISpecificFact> Calculate(EventTracker eventTracker)
        {
            var loadAllTrackerEvents = _eventRepository.LoadAllTrackerEvents(eventTracker.Id);
            if (!CanCalculate(eventTracker, loadAllTrackerEvents))
            {
                return Option<ISpecificFact>.None;
            }
            var sumScale = loadAllTrackerEvents.Select(x=>x.Scale).Somes().Sum();
            var measurementUnit = eventTracker.ScaleMeasurementUnit.Match(
                x=>x,
                ()=>
                {
                    Log.Error("EventTracker has not scale while calculate inside SumScaleCalculator");
                    return "";
                } );
            return Option<ISpecificFact>.Some(new SumScaleFact(
                "Суммарное значение шкалы",
                $"Сумма значений {measurementUnit} для события {eventTracker.Name} равна {sumScale}",
                PriorityValue,
                sumScale,
                measurementUnit
            ));
        }

        private static bool CanCalculate(EventTracker eventTracker, IReadOnlyList<Event> loadAllTrackerEvents)
        {
            if (eventTracker.ScaleMeasurementUnit.IsNone)
            {
                return false;
            }

            if (loadAllTrackerEvents.Any(@event => @event.Scale == Option<double>.None))
            {
                return false;
            }
            return loadAllTrackerEvents.Count > EventsThreshold;
        }
    }
}