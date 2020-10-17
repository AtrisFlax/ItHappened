using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using NUnit.Framework;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class SpecificDayTimeEventCalculatorTest
    {
        [Test]
        public void CreateEventTrackerWithHeadacheAndSmokingEvents_CalculateSpecificDayTimeEventFact_CheckAProperties()
        {
            var userId = Guid.NewGuid();

            var headacheEventMorning1 = CreateEventWithNameAndDateTime(userId, "headache", "2020.10.8 01:05:00");
            var headacheEventMorning2 = CreateEventWithNameAndDateTime(userId, "headache", "2020.11.9 02:05:00");
            var headacheEventMorning3 = CreateEventWithNameAndDateTime(userId, "headache", "2020.12.9 03:07:00");
            var headacheEventMorning4 = CreateEventWithNameAndDateTime(userId, "headache", "2020.10.3 4:05:00");
            var headacheEventMorning5 = CreateEventWithNameAndDateTime(userId, "headache", "2021.10.9 05:05:00");
            var headacheEventMorning6 = CreateEventWithNameAndDateTime(userId, "headache", "2020.10.9 10:05:00");

            var smokingEventMorning1 = CreateEventWithNameAndDateTime(userId, "smoking", "2020.10.9 04:05:00");
            var smokingEventMorning2 = CreateEventWithNameAndDateTime(userId, "smoking", "2020.10.9 09:05:00");

            var events = new List<Event>
            {
                headacheEventMorning1, headacheEventMorning2, headacheEventMorning3, headacheEventMorning6,
                headacheEventMorning4, headacheEventMorning5, smokingEventMorning1, smokingEventMorning2
            };
            var eventTracker = new EventTracker(Guid.NewGuid(), "nameEmpty", events, userId);

            var specificDayTimeEventFact = new SpecificDayTimeEventCalculator()
                .Calculate(eventTracker);

            Assert.AreEqual(true, specificDayTimeEventFact.IsSome);
            specificDayTimeEventFact.Do(e =>
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

        private Event CreateEventWithNameAndDateTime(Guid userId, string title, string dateTime)
        {
            return new Event(Guid.NewGuid(), userId, title)
            {
                HappensDate = DateTime.Parse(dateTime)
            };
        }
    }
}