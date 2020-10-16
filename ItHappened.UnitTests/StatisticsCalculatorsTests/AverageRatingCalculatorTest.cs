using System;
using System.Collections.Generic;
using System.Linq;
using ItHappend.Domain.Statistics.SingleTrackerCalculator;
using ItHappened.Domain;
using ItHappened.Domain.EventTracker;
using NUnit.Framework;

namespace ItHappend.UnitTests.StatisticsCalculatorsTests
{
    public class AverageRatingCalculatorTest
    {
        [Test]
        public void EventTrackerHasTwoRatingAndEvents_CalculateSucess()
        {

            var ratings = new List<double> {2.0, 5.0};
            //arrange 
            var eventList =
                new List<Event>
                {
                    EventBuilder
                        .Event(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow, "Event1").WithRating(ratings[0])
                        .Build(),
                    EventBuilder
                        .Event(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow, "Event1").WithRating(ratings[1])
                        .Build()
                };
            var eventTracker = new EventTracker(Guid.NewGuid(), "TrackerName", eventList, Guid.NewGuid(), hasRating : true);
            
            //act 
            var fact = new AverageRatingCalculator().Calculate(eventTracker);

            //assert 
            Assert.True(fact.IsSome);
            fact.Do(f =>
            {
                Assert.AreEqual(Math.Sqrt(ratings.Average()), f.Priority);
                Assert.AreEqual(ratings.Average(), f.AverageRating);
            });
        }
    }
}