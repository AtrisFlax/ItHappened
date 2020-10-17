using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics.Calculators.ForMultipleTrackers;
using NUnit.Framework;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class MostFrequentEventCalculatorTest
    {
        [Test]
        public void CreateTwoEventTrackersWithHeadacheAndToothacheEvents_GetMostFrequentEventFact_CheckAllProperties()
        {
            var userId = Guid.NewGuid();
            
            var headacheEventYesterday = CreateEventWithNameAndDateTime(userId, "headache", DateTime.Now.AddDays(-1));
            var headacheEventYesterdayAgain = CreateEventWithNameAndDateTime(userId, "headache", DateTime.Now.AddDays(-1));
            var headacheEventTwoDaysAgo = CreateEventWithNameAndDateTime(userId, "headache", DateTime.Now.AddDays(-2));
            var headacheEventThreeDaysAgo = CreateEventWithNameAndDateTime(userId, "headache", DateTime.Now.AddDays(-3));

            var toothacheEventYesterday = CreateEventWithNameAndDateTime(userId, "toothache", DateTime.Now.AddDays(-1));
            var toothacheEventTwoDaysAgo = CreateEventWithNameAndDateTime(userId, "toothache", DateTime.Now.AddDays(-2));
            var toothacheEventTwoDaysAgoAgain = CreateEventWithNameAndDateTime(userId, "toothache", DateTime.Now.AddDays(-2));

            var events1 = new List<Event> 
                {headacheEventYesterday, headacheEventTwoDaysAgo, headacheEventThreeDaysAgo, toothacheEventYesterday};
            var events2 = new List<Event> 
                {headacheEventYesterday, headacheEventYesterdayAgain, toothacheEventTwoDaysAgo, 
                    toothacheEventTwoDaysAgoAgain, headacheEventTwoDaysAgo};
            
            var eventTracker1 = new EventTracker(Guid.NewGuid(), "Pains after drinking sugar water", events1, userId);
            var eventTracker2 = new EventTracker(Guid.NewGuid(), "Pains after eating sugar", events2, userId);

            var mostFrequentEvent = new MostFrequentEventCalculator()
                .Calculate(new[] {eventTracker1, eventTracker2});
            
            Assert.IsTrue(mostFrequentEvent.IsSome);
            mostFrequentEvent.Do(e =>
            {
                Assert.AreEqual("Чаще всего у вас происходит событие \"Pains after eating sugar\"" + 
                                " - раз в 0,4 дней", e.Description);
                Assert.AreEqual(25, e.Priority);
                Assert.AreEqual("Pains after eating sugar", e.TrackingName);
                Assert.AreEqual(0.4, e.EventsPeriod, 0.01);
                Assert.AreEqual(0.75,e.EventTrackersWithPeriods
                    .First(q => q.TrackingName == "Pains after drinking sugar water")
                    .EventPeriod, 0.01);
            });

        }
        
        private Event CreateEventWithNameAndDateTime(Guid userId, string title, DateTime dateTime)
        {
            return new Event(Guid.NewGuid(), userId, title)
            {
                HappensDate = dateTime
            };
        }
        
    }
}