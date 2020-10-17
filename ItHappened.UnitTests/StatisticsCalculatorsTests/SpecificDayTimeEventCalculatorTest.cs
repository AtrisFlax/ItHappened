using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics.Calculators.ForSingleTracker;
using NUnit.Framework;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class SpecificDayTimeEventCalculatorTest
    {
        private List<Event> _events;
        
        [SetUp] 
        public void Setup()
        {
        }

        private Event Create(string title, Guid userId, string dateTime)
        {
            return new Event(Guid.NewGuid(), userId, title, new Random().Next(1, 9))
            {
                HappensDate = DateTime.Parse("2020.10.8 01:05:00")
            };
        }

        [Test]
        public void TwoEventTrackers_CreateMostFrequentEvent_GetFourTrueResult()
        {
            var userId = Guid.NewGuid();

            var headacheEvent1 = new Event(Guid.NewGuid(), userId, "headache", 3)
            {
                HappensDate = DateTime.Parse("2020.10.8 01:05:00")
            };
            var headacheEvent2 = new Event(Guid.NewGuid(), userId, "headache", 10)
            {
                HappensDate = DateTime.Parse("2020.11.9 02:05:00")
            };
            var headacheEvent3 = new Event(Guid.NewGuid(), userId, "headache", 7)
            {
                HappensDate = DateTime.Parse("2020.12.9 03:07:00")
            };
            var headacheEvent4 = new Event(Guid.NewGuid(), userId, "headache", 2.9)
            {
                HappensDate = DateTime.Parse("2020.10.3 10:05:00")
            };
            var headacheEvent5 = new Event(Guid.NewGuid(), userId, "headache", 3)
            {
                HappensDate = DateTime.Parse("2021.10.9 05:05:00")
            };
            var headacheEvent6 = new Event(Guid.NewGuid(), userId, "headache", 1)
            {
                HappensDate = DateTime.Parse("2020.10.9 04:05:00")
            };
            var smokingEvent3 = new Event(Guid.NewGuid(), userId, "smoking", 4.6)
            {
                HappensDate = DateTime.Parse("2020.10.9 04:05:00")
            };
            var smokingEvent4 = new Event(Guid.NewGuid(), userId, "smoking", 4.6)
            {
                HappensDate = DateTime.Parse("2020.10.9 09:05:00")
            };

            var events = new List<Event> 
                {headacheEvent1, headacheEvent2, headacheEvent3, headacheEvent6, headacheEvent4, headacheEvent5, 
                    smokingEvent3, smokingEvent4};
            var eventTracker = new EventTracker(Guid.NewGuid(), "name1", events, userId);

            var mostFrequentEvent = new SpecificDayTimeEventCalculator()
                .Calculate(eventTracker);

            Assert.AreEqual(true, mostFrequentEvent.IsSome);
            mostFrequentEvent.Do(e =>
                {
                    Assert.AreEqual("В 83% случаев событие \"headache\" происходит night", e.Description);
                    Assert.AreEqual(11.66, e.Priority, 0.01);
                    Assert.AreEqual("SpecificTimeOfDayEventFact", e.FactName);
                    Assert.AreEqual("night", e.TimeOfTheDay);
                    Assert.AreEqual(50, e.VisualizationData
                        .First(q => q.Title == "smoking" && q.TimeOfTheDay == "morning")
                        .Percentage);
                    Assert.AreEqual(16.66, e.VisualizationData
                        .First(q => q.Title == "headache" && q.TimeOfTheDay == "morning")
                        .Percentage, 0.01);
                }
            );
        }
    }
}