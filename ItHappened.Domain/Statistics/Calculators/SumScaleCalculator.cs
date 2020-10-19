using System.Linq;
using ItHappend.Domain.Statistics;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Domain.Statistics
{
    public class SumScaleCalculator : ISingleTrackerStatisticsCalculator
    {
        private readonly IEventRepository _eventRepository;
        public SumScaleCalculator(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        public Option<IStatisticsFact> Calculate(EventTracker eventTracker)
        {
            if (!CanCalculate(eventTracker)) return Option<IStatisticsFact>.None;
            var sumScale = _eventRepository.LoadAllTrackerEvents(eventTracker.Id).Sum(x => x.Scale.ValueUnsafe());
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

            if (_eventRepository.LoadAllTrackerEvents(eventTracker.Id).Any(@event => @event.Scale == Option<double>.None))
            {
                return false;
            }

            return _eventRepository.LoadAllTrackerEvents(eventTracker.Id).Count > 1;
        }
    }
}