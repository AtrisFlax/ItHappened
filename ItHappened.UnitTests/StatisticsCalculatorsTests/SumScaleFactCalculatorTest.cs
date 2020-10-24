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
    public class SumScaleFactCalculatorTest
    {
        private IEventRepository _eventRepository;
        private static Random _rand;
        private const int MinEventForCalculation = 2;
        private const string MeasurementUnit = "Kg";

        [SetUp]
        public void Init()
        {
            _eventRepository = new EventRepository();
            _rand = new Random();
        }

        [Test]
        public void EventsEnoughWithScaleEventsForCalc_CalculateSuccess()
        {
            //arrange 
            var userId = Guid.NewGuid();
            var tracker = CreateTrackerWithScale(userId,MeasurementUnit);
            var (events, scaleValues) =
                CreateEventsWithScale(tracker.Id, _rand.Next() % 10 + MinEventForCalculation);
            _eventRepository.AddRangeOfEvents(events);
            var allEvents = _eventRepository.LoadAllTrackerEvents(tracker.Id);

            //act 
            var fact = new SumScaleCalculator()
                .Calculate(allEvents, tracker).ConvertTo<SumScaleTrackerFact>().ValueUnsafe();

            //assert 
            Assert.AreEqual(2, fact.Priority, PriorityAccuracy);
            Assert.AreEqual(scaleValues.Sum(), fact.SumValue);
            Assert.AreEqual(MeasurementUnit, fact.MeasurementUnit);
        }

        [Test]
        public void EventTrackerHasOneEvent_CalculateFailure()
        {
            //arrange 
            var userId = Guid.NewGuid();
            var eventTracker = CreateTrackerWithScale(userId,MeasurementUnit);
            var (events, _) = CreateEventsWithScale(eventTracker.Id, MinEventForCalculation);
            _eventRepository.AddRangeOfEvents(events);

            //act 
            var fact = new SumScaleCalculator().Calculate(events, eventTracker)
                .ConvertTo<SumScaleTrackerFact>();

            //assert 
            Assert.True(fact.IsNone);
        }
    }
}