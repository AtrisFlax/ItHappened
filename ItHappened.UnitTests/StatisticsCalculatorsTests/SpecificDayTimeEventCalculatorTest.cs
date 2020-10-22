using System;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure.Repositories;
using NUnit.Framework;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class SpecificDayTimeEventCalculatorTest
    {
        private IEventRepository _eventRepository;
        
        [SetUp]
        public void Init()
        {
            _eventRepository = new EventRepository();
        }
        
        [Test]
        public void CreateEventTrackerWithHeadacheAndSmokingEvents_CalculateSpecificDayTimeEventFact_CheckAProperties()
        {
            var userId = Guid.NewGuid();
            var eventTracker = EventTrackerBuilder
                .Tracker(userId, Guid.NewGuid(), "nameEmpty")
                .Build();
            var headacheEventMorning1 = CreateEventWithNameAndDateTime(userId, eventTracker.Id, "headache", "2020.10.8 01:05:00");
            var headacheEventMorning2 = CreateEventWithNameAndDateTime(userId, eventTracker.Id,"headache", "2020.11.9 02:05:00");
            var headacheEventMorning3 = CreateEventWithNameAndDateTime(userId, eventTracker.Id,"headache", "2020.12.9 03:07:00");
            var headacheEventMorning4 = CreateEventWithNameAndDateTime(userId, eventTracker.Id,"headache", "2020.10.3 4:05:00");
            var headacheEventMorning5 = CreateEventWithNameAndDateTime(userId, eventTracker.Id,"headache", "2021.10.9 05:05:00");
            var headacheEventMorning6 = CreateEventWithNameAndDateTime(userId, eventTracker.Id,"headache", "2020.10.9 10:05:00");

            var smokingEventMorning1 = CreateEventWithNameAndDateTime(userId, eventTracker.Id,"smoking", "2020.10.9 04:05:00");
            var smokingEventMorning2 = CreateEventWithNameAndDateTime(userId, eventTracker.Id,"smoking", "2020.10.9 09:05:00");

            _eventRepository.AddRangeOfEvents(new []
            {
                headacheEventMorning1, headacheEventMorning2, headacheEventMorning3,
                headacheEventMorning4, headacheEventMorning5, headacheEventMorning6, 
                smokingEventMorning1, smokingEventMorning2 
            });
            
            var specificDayTimeEventFact = new SingleTrackerStatisticsDayTimeEventCalculator(_eventRepository)
                .Calculate(eventTracker)
                .ConvertTo<SingleTrackerTimeOfDayEventFact>();

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

        private static Event CreateEventWithNameAndDateTime(Guid userId, Guid trackerId, string title, string dateTime)
        {
            return EventBuilder.Event(Guid.NewGuid(), userId, trackerId, DateTime.Parse(dateTime), title).Build();
        }
    }
}