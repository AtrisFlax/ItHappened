﻿using System.Collections.Generic;
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
            var loadAllTrackerEvents = _eventRepository.LoadAllTrackerEvents(eventTracker.Id);
            if (!CanCalculate(eventTracker, loadAllTrackerEvents))
            {
                return Option<IStatisticsFact>.None;
            }
            var sumScale = loadAllTrackerEvents.Select(x=>x.Scale).Somes().Sum();
            var measurementUnit = eventTracker.ScaleMeasurementUnit.ValueUnsafe();
            return Option<IStatisticsFact>.Some(new SumScaleFact(
                "Суммарное значение шкалы",
                $"Сумма значений {measurementUnit} для события {eventTracker.Name} равна {sumScale}",
                2.0,
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
            return loadAllTrackerEvents.Count > 1;
        }
    }
}