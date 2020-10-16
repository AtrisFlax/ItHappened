using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace ItHappened.UnitTests
{
    public class FactStatisticsTests
    {
        [SetUp]
        public void Setup()
        {
        }

        /*[Test]
        public void TwoEventTrackers_CreateMostFrequentEvent_GetFourTrueResult()
        {
            var userId = Guid.NewGuid();
            var headacheEventId = Guid.NewGuid();
            var headacheEvent1 = new Event(Guid.NewGuid(), userId, "headache", 3)
            {
                HappensDate = DateTimeOffset.Now.AddDays(-1)
            };
            var headacheEvent2 = new Event(Guid.NewGuid(), userId, "headache", 10)
            {
                HappensDate = DateTimeOffset.Now.AddDays(-1)
            };
            var headacheEvent3 = new Event(Guid.NewGuid(), userId, "headache", 7)
            {
                HappensDate = DateTimeOffset.Now.AddDays(-2)
            };
            var headacheEvent4 = new Event(Guid.NewGuid(), userId, "headache", 2.9)
            {
                HappensDate = DateTimeOffset.Now.AddDays(-3)
            };
            var smokingEvent1 = new Event(Guid.NewGuid(), userId, "smoking", 3);
            var smokingEvent2 = new Event(Guid.NewGuid(), userId, "smoking", 1);
            var smokingEvent3 = new Event(Guid.NewGuid(), userId, "smoking", 4.6)
            {
                HappensDate = DateTimeOffset.Now.AddDays(-3)
            };

            var events1 = new List<Event>
                {headacheEvent1, headacheEvent3, headacheEvent4, smokingEvent1};
            var events2 = new List<Event>
                {headacheEvent1, headacheEvent2, smokingEvent2, smokingEvent3, headacheEvent3};

            var eventTracker1 = new EventTracker(Guid.NewGuid(), "name1", events1, userId);
            var eventTracker2 = new EventTracker(Guid.NewGuid(), "name2", events2, userId);

            var mostFrequentEvent = MostFrequentEvent
                .CreateFactStatistics(new[] {eventTracker1, eventTracker2});
            
            Assert.AreEqual
                ("Чаще всего у вас происходит событие name2 - раз в 0,8 дней", mostFrequentEvent.Item1.Description);
            Assert.AreEqual(12.5, mostFrequentEvent.Item1.Priority);
            Assert.AreEqual("name2", mostFrequentEvent.Item1.TrackingName);
            Assert.AreEqual(0.8, mostFrequentEvent.Item1.EventsPeriod, 0.01);
        }*/
    }
}