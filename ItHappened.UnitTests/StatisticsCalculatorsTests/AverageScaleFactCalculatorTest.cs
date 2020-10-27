using System;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure.Repositories;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.StatisticsCalculatorsTestingConsts;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.TestingMethods;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class AverageScaleFactCalculatorTest
    {
        private IEventRepository _eventRepository;
        private static Random _rand;
        private DateTimeOffset _now;
        private const int MinEventForCalculation = 2;
        private const string MeasurementUnit = "Kg";

        [SetUp]
        public void Init()
        {
            _eventRepository = new EventRepository();
            _rand = new Random();
            _now = DateTimeOffset.UtcNow;
        }

        [Test]
        public void EventsOnlyWithScale_CalculateSuccess()
        {
            //arrange 
            var userId = Guid.NewGuid();
            var tracker = CreateTrackerWithScale(userId, MeasurementUnit);
            var (events, scaleValues) =
                CreateEventsWithScale(tracker.Id, _rand.Next() % 10 + MinEventForCalculation);
            _eventRepository.AddRangeOfEvents(events);
            var allEvents = _eventRepository.LoadAllTrackerEvents(tracker.Id);

            //act 
            var fact = new AverageScaleCalculator().Calculate(allEvents, tracker, _now)
                .ConvertTo<AverageScaleTrackerFact>().ValueUnsafe();

            //assert 
            Assert.AreEqual("Среднее значение шкалы", fact.FactName);
            Assert.AreEqual(3, fact.Priority, PriorityAccuracy);
            Assert.AreEqual(MeasurementUnit, fact.MeasurementUnit);
            Assert.AreEqual(scaleValues.Average(), fact.AverageValue, AverageAccuracy);
        }

        [Test]
        public void EventTrackerHasOneEvent_CalculateFailure()
        {
            //arrange 
            var userId = Guid.NewGuid();
            var tracker = CreateTrackerWithScale(userId, MeasurementUnit);
            var (events, _) = CreateEventsWithScale(tracker.Id, 1);
            _eventRepository.AddRangeOfEvents(events);
            var allEvents = _eventRepository.LoadAllTrackerEvents(tracker.Id);
            
            //act 
            var fact = new AverageScaleCalculator().Calculate(allEvents, tracker, _now)
                .ConvertTo<AverageScaleTrackerFact>();

            //assert 
            Assert.True(fact.IsNone);
        }

        [Test]
        public void EventTrackerHasZeroEvent_CalculateFailure()
        {
            //arrange 
            var userId = Guid.NewGuid();
            var tracker = CreateTrackerWithScale(userId, MeasurementUnit);
            var (events, _) = CreateEventsWithScale(tracker.Id, 0);
            _eventRepository.AddRangeOfEvents(events);
            var allEvents = _eventRepository.LoadAllTrackerEvents(tracker.Id);

            //act 
            var fact = new AverageScaleCalculator().Calculate(allEvents, tracker, _now)
                .ConvertTo<AverageScaleTrackerFact>();

            //assert 
            Assert.True(fact.IsNone);
        }
    }
}