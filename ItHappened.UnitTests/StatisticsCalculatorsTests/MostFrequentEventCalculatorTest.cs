using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using NUnit.Framework;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class MostFrequentEventCalculatorTest
    {
        [Test]
        public void CreateTwoEventTrackersWithHeadacheAndToothacheEvents_GetMostFrequentEventFact_CheckAllProperties()
        {
            var userId = Guid.NewGuid();

            var headacheEventYesterday =
                CreateEventWithNameAndDateTime(userId, "headache", DateTimeOffset.Now.AddDays(-1));
            var headacheEventYesterdayAgain =
                CreateEventWithNameAndDateTime(userId, "headache", DateTimeOffset.Now.AddDays(-1));
            var headacheEventTwoDaysAgo =
                CreateEventWithNameAndDateTime(userId, "headache", DateTimeOffset.Now.AddDays(-2));
            var headacheEventThreeDaysAgo =
                CreateEventWithNameAndDateTime(userId, "headache", DateTimeOffset.Now.AddDays(-3));

            var toothacheEventYesterday =
                CreateEventWithNameAndDateTime(userId, "toothache", DateTimeOffset.Now.AddDays(-1));
            var toothacheEventTwoDaysAgo =
                CreateEventWithNameAndDateTime(userId, "toothache", DateTimeOffset.Now.AddDays(-2));
            var toothacheEventTwoDaysAgoAgain =
                CreateEventWithNameAndDateTime(userId, "toothache", DateTimeOffset.Now.AddDays(-2));

            var events1 = new List<Event>
                {headacheEventYesterday, headacheEventTwoDaysAgo, headacheEventThreeDaysAgo, toothacheEventYesterday};
            var events2 = new List<Event>
            {
                headacheEventYesterday, headacheEventYesterdayAgain, toothacheEventTwoDaysAgo,
                toothacheEventTwoDaysAgoAgain, headacheEventTwoDaysAgo
            };

            var eventTracker1 = new EventTracker(userId, Guid.NewGuid(), "Pains after drinking sugar water", events1);
            var eventTracker2 = new EventTracker(userId, Guid.NewGuid(), "Pains after eating sugar", events2);

            var mostFrequentEvent = new MostFrequentEventCalculator().Calculate(new[] {eventTracker1, eventTracker2})
                .ConvertTo<MostFrequentEventFact>();

            Assert.IsTrue(mostFrequentEvent.IsSome);
            mostFrequentEvent.Do(e =>
            {
                Assert.AreEqual("Чаще всего у вас происходит событие \"Pains after eating sugar\"" +
                                " - раз в 0,4 дней", e.Description);
                Assert.AreEqual(25, e.Priority);
                Assert.AreEqual("Pains after eating sugar", e.TrackingName);
                Assert.AreEqual(0.4, e.EventsPeriod, 0.01);
                Assert.AreEqual(0.75, e.EventTrackersWithPeriods
                    .First(q => q.TrackingName == "Pains after drinking sugar water")
                    .EventPeriod, 0.01);
            });
        }

        private Event CreateEventWithNameAndDateTime(Guid userId, string title, DateTimeOffset dateTime)
        {
            return EventBuilder.Event(Guid.NewGuid(), userId, dateTime, title).Build();
        }
    }
}