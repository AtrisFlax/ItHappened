using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using NUnit.Framework;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class AverageRatingCalculatorTest
    {
        [Test]
        public void EventTrackerHasTwoRatingAndEvents_CalculateSuccess()
        {
            //arrange 
            var ratings = new List<double> {2.0, 5.0};
            var eventTracker = EventTrackerBuilder
                .Tracker(Guid.NewGuid(), Guid.NewGuid(), "TrackerName")
                .WithRating()
                .Build();
            var eventList = CreateTwoEvents(eventTracker.Id,ratings);
            foreach (var @event in eventList) eventTracker.AddEvent(@event);

            //act 
            var fact = new AverageRatingCalculator().Calculate(eventTracker).ConvertTo<AverageRatingFact>();
            //assert 
            Assert.True(fact.IsSome);
            fact.Do(f =>
            {
                Assert.AreEqual(Math.Sqrt(ratings.Average()), f.Priority);
                Assert.AreEqual(ratings.Average(), f.AverageRating);
            });
        }


        [Test]
        public void EventTrackerHasNoRationCustomization_CalculateFailed()
        {
            //arrange 
            var eventTracker = EventTrackerBuilder
                .Tracker(Guid.NewGuid(), Guid.NewGuid(), "TrackerName")
                .Build();
            var eventList = CreateTwoEvents(eventTracker.Id);
            foreach (var @event in eventList) eventTracker.AddEvent(@event);

            //act 
            var fact = new AverageRatingCalculator().Calculate(eventTracker).ConvertTo<AverageRatingFact>();

            //assert 
            Assert.True(fact.IsNone);
        }

        [Test]
        public void EventTrackerHasOneEvent_CalculateFailed()
        {
            //arrange 
            var ratings = new List<double> {2.0};
            var eventTracker = EventTrackerBuilder
                .Tracker(Guid.NewGuid(), Guid.NewGuid(), "TrackerName")
                .WithRating()
                .Build();
            var @event = EventBuilder
                .Event(Guid.NewGuid(), Guid.NewGuid(), eventTracker.Id, DateTimeOffset.UtcNow, "Event1").WithRating(ratings[0])
                .Build();
            eventTracker.AddEvent(@event);

            //act 
            var fact = new AverageRatingCalculator().Calculate(eventTracker).ConvertTo<AverageRatingFact>();

            //assert 
            Assert.True(fact.IsNone);
        }


        [Test]
        public void SomeEventHasNoCustomizationRating_CalculateFailed()
        {
            //arrange 
            var eventTracker = EventTrackerBuilder
                .Tracker(Guid.NewGuid(), Guid.NewGuid(), "TrackerName")
                .Build();
            var eventList = CreateTwoEventOneOfThemWithoutRating(eventTracker.Id);
            foreach (var @event in eventList) eventTracker.AddEvent(@event);

            //act 
            var fact = new AverageRatingCalculator().Calculate(eventTracker).ConvertTo<AverageRatingFact>();

            //assert 
            Assert.True(fact.IsNone);
        }

        private static IEnumerable<Event> CreateTwoEventOneOfThemWithoutRating(Guid trackerId)
        {
            var ratings = new List<double> {2.0};
            return new List<Event>
            {
                EventBuilder
                    .Event(Guid.NewGuid(), Guid.NewGuid(), trackerId, DateTimeOffset.UtcNow, "Event1")
                    .Build(),
                EventBuilder
                    .Event(Guid.NewGuid(), Guid.NewGuid(), trackerId, DateTimeOffset.UtcNow, "Event2")
                    .WithRating(ratings[0])
                    .Build()
            };
        }

        private static IEnumerable<Event> CreateTwoEvents(Guid trackerId)
        {
            var ratings = new List<double> {2.0, 5.0};
            return new List<Event>
            {
                EventBuilder
                    .Event(Guid.NewGuid(), Guid.NewGuid(), trackerId,DateTimeOffset.UtcNow, "Event1").WithRating(ratings[0])
                    .Build(),
                EventBuilder
                    .Event(Guid.NewGuid(), Guid.NewGuid(), trackerId,DateTimeOffset.UtcNow, "Event1").WithRating(ratings[1])
                    .Build()
            };
        }

        private static IEnumerable<Event> CreateTwoEvents(Guid trackerId, IList<double> ratings)
        {
            return new List<Event>
            {
                EventBuilder
                    .Event(Guid.NewGuid(), Guid.NewGuid(), trackerId,DateTimeOffset.UtcNow, "Event1").WithRating(ratings[0])
                    .Build(),
                EventBuilder
                    .Event(Guid.NewGuid(), Guid.NewGuid(), trackerId,DateTimeOffset.UtcNow, "Event1").WithRating(ratings[1])
                    .Build()
            };
        }
    }
}