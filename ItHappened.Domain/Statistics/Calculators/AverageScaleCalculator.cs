using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Domain.Statistics
{
    public class AverageScaleCalculator : ISpecificCalculator
    {
        private const double PriorityValue = 3.0;
        private readonly IEventRepository _eventRepository;

        public AverageScaleCalculator(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public Option<ISpecificFact> Calculate(EventTracker eventTracker)
        {
            var events = _eventRepository.LoadAllTrackerEvents(eventTracker.Id);
            if (!CanCalculate(eventTracker, events)) return Option<ISpecificFact>.None;
            var averageValue = events.Select(x=>x.Scale).Somes().Average();
            var measurementUnit = eventTracker.ScaleMeasurementUnit.ValueUnsafe();
            return Option<ISpecificFact>.Some(new AverageScaleFact(
                "Среднее значение шкалы",
                $"Сумма значений {measurementUnit} для события {eventTracker.Name} равно {averageValue}",
                PriorityValue, 
                averageValue,
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

            return loadAllTrackerEvents.Count > 1;
        }
    }
}