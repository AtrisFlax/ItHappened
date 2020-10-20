using System;
using System.Collections.Generic;
using System.Linq;
using ItHappend.Domain.Statistics;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure.Repositories;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.StatisticsCalculatorsTestingConsts;

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
        public void EventTrackerHasTwoRatingAndEvents_CalculateSuccess()
        {
            //arrange 
            var eventTracker = CreateTrackerWithScale(MeasurementUnit);
            var (events, scaleValues) = CreateEventsWithScale(eventTracker.Id, _rand.Next() % 10 + MinEventForCalculation);
            _eventRepository.AddRangeOfEvents(events);
            
            //act 
            var fact = new SumScaleCalculator(_eventRepository)
                .Calculate(eventTracker).ConvertTo<SumScaleFact>().ValueUnsafe();
            
            //assert 
            Assert.AreEqual(2, fact.Priority, PriorityAccuracy);
            Assert.AreEqual(scaleValues.Sum(), fact.SumValue);
            Assert.AreEqual(MeasurementUnit, fact.MeasurementUnit);
        }

        [Test]
        public void EventTrackerHasOneEvent_CalculateFailure()
        {
            //arrange 
            var eventTracker = CreateTrackerWithScale(MeasurementUnit);
            var (events, _) = CreateEventsWithScale(eventTracker.Id, 1);
            _eventRepository.AddRangeOfEvents(events);
                
            //act 
            var fact = new SumScaleCalculator(_eventRepository).Calculate(eventTracker).ConvertTo<SumScaleFact>();

            //assert 
            Assert.True(fact.IsNone);
        }

        [Test]
        public void EventTrackerHasNoScaleCustomization_CalculateFailure()
        {
            //arrange 
            var eventTracker = CreateTrackerWithoutScale(MeasurementUnit);
            var (events, _) = CreateEventsWithScale(eventTracker.Id, _rand.Next() % 10 + MinEventForCalculation);
            _eventRepository.AddRangeOfEvents(events);

            //act 
            var fact = new SumScaleCalculator(_eventRepository).Calculate(eventTracker).ConvertTo<SumScaleFact>();

            //assert 
            Assert.True(fact.IsNone);
        }
        
        [Test]
        public void SomeEventHasNoCustomizationScale_CalculateFailed()
        {
            //arrange 
            var eventTracker = CreateTrackerWithScale(MeasurementUnit);
            var (events, _) = CreateAllEventsWithScaleButOneWithoutScale(eventTracker.Id, 1);
            _eventRepository.AddRangeOfEvents(events);
            
            //act 
            var fact = new SumScaleCalculator(_eventRepository).Calculate(eventTracker).ConvertTo<SumScaleFact>();

            //assert 
            Assert.True(fact.IsNone);
        }
        
        private static (IEnumerable<Event> events, IEnumerable<double> scaleValues) CreateEventsWithScale(Guid trackerId, int num)
        {
            var scaleValues = CreateRandomScale(num);
            var events = scaleValues.Select(t => CreateEventWithScale(trackerId, t)).ToList();
            return (events, scaleValues);
        }
        
        // ReSharper disable once UnusedTupleComponentInReturnValue
        private static (IEnumerable<Event> events, IEnumerable<double> scaleValues) CreateAllEventsWithScaleButOneWithoutScale(Guid trackerId, int num)
        {
            var scaleValues = CreateRandomScale(num);
            var events = scaleValues.Select(t => CreateEventWithScale(trackerId, t)).ToList();
            events.Add(EventBuilder
                .Event(Guid.NewGuid(), Guid.NewGuid(), trackerId, DateTimeOffset.UtcNow, "Event No Scale")
                .Build());
            return (events, scaleValues);
        }
        
        private static List<double> CreateRandomScale(int num)
        {
            var ratings = new List<double>();
            for (var i = 0; i < num; i++)
            {
                ratings.Add(_rand.NextDouble());
            }
            return ratings;
        }
        
        private static Event CreateEventWithScale(Guid trackerId, double scale)
        {
            return EventBuilder
                .Event(Guid.NewGuid(), Guid.NewGuid(), trackerId, DateTimeOffset.UtcNow, $"Event with scale {scale}")
                .WithScale(scale)
                .Build();
        }
        
        private static EventTracker CreateTrackerWithScale(string measurementUnit)
        {
            var eventTracker = EventTrackerBuilder
                .Tracker(Guid.NewGuid(), Guid.NewGuid(), $"Tracker with scale {measurementUnit}")
                .WithScale(measurementUnit)
                .Build();
            return eventTracker;
        }
        
        private static EventTracker CreateTrackerWithoutScale(string measurementUnit)
        {
            var eventTracker = EventTrackerBuilder
                .Tracker(Guid.NewGuid(), Guid.NewGuid(), $"Tracker with scale {measurementUnit}")
                .Build();
            return eventTracker;
        }
    }
}

