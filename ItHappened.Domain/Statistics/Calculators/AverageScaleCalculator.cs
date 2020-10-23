using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Domain.Statistics
{
    public class AverageScaleCalculator : ISingleTrackerStatisticsCalculator
    {
        private readonly IEventRepository _eventRepository;

        public AverageScaleCalculator(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public Option<ISingleTrackerFact> Calculate(EventTracker eventTracker)
        {
            var events = _eventRepository.LoadAllTrackerEvents(eventTracker.Id);
            if (!CanCalculate(eventTracker, events)) return Option<ISingleTrackerFact>.None;
            var averageValue = events.Select(x=>x.CustomParameters.Scale).Somes().Average();
            var measurementUnit = eventTracker.CustomizationSettings.ScaleMeasurementUnit.ValueUnsafe();
            return Option<ISingleTrackerFact>.Some(new AverageScaleFact(
                "Среднее значение шкалы",
                $"Сумма значений {measurementUnit} для события {eventTracker.Name} равно {averageValue}",
                3.0, 
                averageValue,
                measurementUnit
            ));
        }

        private static bool CanCalculate(EventTracker eventTracker, IReadOnlyList<Event> loadAllTrackerEvents)
        {
            if (eventTracker.CustomizationSettings.ScaleMeasurementUnit.IsNone)
            {
                return false;
            }

            if (loadAllTrackerEvents.Any(@event => @event.CustomParameters.Scale == Option<double>.None))
            {
                return false;
            }

            return loadAllTrackerEvents.Count > 1;
        }
    }
}