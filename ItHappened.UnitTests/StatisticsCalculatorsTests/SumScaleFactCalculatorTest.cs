using System;
using System.Collections.Generic;
using System.Linq;
using ItHappend.Domain.Statistics;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using NUnit.Framework;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class SumScaleFactCalculatorTest
    {
        [Test]
        public void EventTrackerHasTwoRatingAndEvents_CalculateSuccess()
        {
            //arrange 
            var scaleValues = new List<double> {2.0, 5.0};
            const string measurementUnit = "Kg";
            var eventList =
                new List<Event>
                {
                    EventBuilder
                        .Event(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow, "Event1")
                        .WithScale(scaleValues[0])
                        .Build(),
                    EventBuilder
                        .Event(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow, "Event2")
                        .WithScale(scaleValues[1])
                        .Build()
                };
            var eventTracker = EventTrackerBuilder
                .Tracker(Guid.NewGuid(), Guid.NewGuid(), "TrackerName")
                .WithScale(measurementUnit)
                .Build();
            foreach (var @event in eventList)
            {
                eventTracker.AddEvent(@event);
            }

            //act 
            var fact = new SumScaleCalculator().Calculate(eventTracker).ConvertTo<SumScaleFact>();
            
            //assert 
            Assert.True(fact.IsSome);
            fact.Do(f =>
            {
                Assert.AreEqual(2, f.Priority);
                Assert.AreEqual(scaleValues.Sum(), f.SumValue);
                Assert.AreEqual(measurementUnit, f.MeasurementUnit);
            });
        }

        [Test]
        public void EventTrackerHasOneEvent_CalculateFailure()
        {
            //arrange 
            var scaleValues = new List<double> {2.0};
            const string measurementUnit = "Kg";
            var eventList =
                new List<Event>
                {
                    EventBuilder
                        .Event(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow, "Event1")
                        .WithScale(scaleValues[0])
                        .Build(),
                };
            var eventTracker = EventTrackerBuilder
                .Tracker(Guid.NewGuid(), Guid.NewGuid(), "TrackerName")
                .WithScale(measurementUnit)
                .Build();
            foreach (var @event in eventList)
            {
                eventTracker.AddEvent(@event);
            }

            //act 
            var fact = new SumScaleCalculator().Calculate(eventTracker).ConvertTo<SumScaleFact>();

            //assert 
            Assert.True(fact.IsNone);
        }

        [Test]
        public void EventTrackerHasNoScaleCustomization_CalculateFailure()
        {
            //arrange 
            var scaleValues = new List<double> {2.0};
            var eventList =
                new List<Event>
                {
                    EventBuilder
                        .Event(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow, "Event1")
                        .WithScale(scaleValues[0])
                        .Build(),
                };
            var eventTracker = EventTrackerBuilder
                .Tracker(Guid.NewGuid(), Guid.NewGuid(), "TrackerName")
                .Build();
            foreach (var @event in eventList)
            {
                eventTracker.AddEvent(@event);
            }

            //act 
            var fact = new SumScaleCalculator().Calculate(eventTracker).ConvertTo<SumScaleFact>();

            //assert 
            Assert.True(fact.IsNone);
        }
        
        [Test]
        public void SomeEventHasNoCustomizationScale_CalculateFailed()
        {
            //arrange 
            var scaleValues = new List<double> {2.0};
            const string measurementUnit = "Kg";
            var eventList =
                new List<Event>
                {
                    EventBuilder
                        .Event(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow, "Event1")
                        .WithScale(scaleValues[0])
                        .Build(),
                    EventBuilder
                        .Event(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow, "Event2")
                        .Build()
                };
            var eventTracker = EventTrackerBuilder
                .Tracker(Guid.NewGuid(), Guid.NewGuid(), "TrackerName")
                .WithScale(measurementUnit)
                .Build();
            foreach (var @event in eventList)
            {
                eventTracker.AddEvent(@event);
            }

            //act 
            var fact = new SumScaleCalculator().Calculate(eventTracker).ConvertTo<SumScaleFact>();

            //assert 
            Assert.True(fact.IsNone);
        }
    }
}

