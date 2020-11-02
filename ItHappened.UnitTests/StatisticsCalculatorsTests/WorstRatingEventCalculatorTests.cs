using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure;
using ItHappened.Infrastructure.Repositories;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.TestingMethods;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.StatisticsCalculatorsTestingConstants;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class WorstEventCalculatorTests
    {
        private const int EventsNumber = 9;
        private Guid _creatorId;
        private IReadOnlyCollection<Event> _events;
        private EventTracker _tracker;
        private WorstRatingEventCalculator _worstEventCalculator;
        private IEventRepository _eventRepository;
        private DateTimeOffset _now;

        private const int MinEventForCalculation = 11;
        private const int MonthsThreshold = 3;
        private const int DaysSinceWorstEventThreshold = 7;
        private const int MaxRating = 10;


        [SetUp]
        public void Init()
        {
            _eventRepository = new EventRepository();
            _creatorId = Guid.NewGuid();
            _tracker = CreateTrackerWithDefaultCustomization(_creatorId);
            _events = CreateEvents(_creatorId, _creatorId, EventsNumber);
            _eventRepository.AddRangeOfEvents(_events);
            _worstEventCalculator = new WorstRatingEventCalculator();
            _now = DateTimeOffset.Now;
        }


        [Test]
        public void CalculateLessThanRequiredNumberEvents_ReturnNone()
        {
            //arrange
            var events = CreateEvents(_tracker.Id, _creatorId, EventsNumber).ToList();
            events.Add(CreateEventWithFixTime(_tracker.Id, _creatorId, _now - TimeSpan.FromDays(91)));
            _eventRepository.AddRangeOfEvents(events);
            var allEvents = _eventRepository.LoadAllTrackerEvents(_tracker.Id);

            //act
            var fact = _worstEventCalculator.Calculate(allEvents, _tracker, _now).ConvertTo<AverageRatingTrackerFact>();

            //assert
            Assert.True(fact.IsNone);
        }

        [Test]
        public void CalculateWithoutOldEnoughEvent_ReturnNone()
        {
            //arrange
            var events = CreateEvents(_tracker.Id, _creatorId, EventsNumber).ToList();
            events.Add(CreateEventWithRatingAndFixDate(_tracker.Id, _creatorId, 1, _now - TimeSpan.FromDays(8)));
            _eventRepository.AddRangeOfEvents(events);
            var allEvents = _eventRepository.LoadAllTrackerEvents(_tracker.Id);

            //act
            var fact = _worstEventCalculator.Calculate(allEvents, _tracker, _now);

            //assert
            Assert.True(fact.IsNone);
        }

        [Test]
        public void CalculateWhenWorstEventHappenedLessThanWeekAgo_ReturnNone()
        {
            //arrange
            var events = CreateEvents(_tracker.Id, _creatorId, EventsNumber).ToList();
            events.Add(CreateEventWithFixTime(_tracker.Id, _creatorId, _now - TimeSpan.FromDays(91)));
            events.Add(CreateEventWithRatingAndFixDate(_tracker.Id, _creatorId, 1.0, _now - TimeSpan.FromDays(6)));
            _eventRepository.AddRangeOfEvents(events);
            var allEvents = _eventRepository.LoadAllTrackerEvents(_tracker.Id);
            //act
            var fact = _worstEventCalculator.Calculate(allEvents, _tracker, _now);

            //assert
            Assert.True(fact.IsNone);
        }

        [Repeat(1000)]
        [Test]
        public void EarliestEventsAfterThreeMonthAfterMoreThen7DaysAgo_CalculateSuccess()
        {
            //assert
            var userId = Guid.NewGuid();
            var now = DateTimeOffset.Now;
            var tracker = CreateTrackerWithScale(userId, "Kg");
            var (events, _, _) =
                CreateEventsWithCommentAndWithRatingFromTo(tracker.Id, userId, MinEventForCalculation,
                    now.AddMonths(-MonthsThreshold),
                    now.AddDays(-DaysSinceWorstEventThreshold));
            var eventsList = events.ToList();
            var eventBefore3Month = CreateEventWithRatingWithCommentAndFixDate(tracker.Id, userId, MaxRatingValue,
                new Comment("Comment"), now.AddMonths(-MonthsThreshold).AddDays(-1));
            eventsList.Add(eventBefore3Month);
            var worstRatingEventInfo
                = events
                    .Where(@event => @event.CustomizationsParameters.Rating.IsSome)
                    .Select(x => new
                    {
                        Event = x,
                        Rating = x.CustomizationsParameters.Rating.ValueUnsafe()
                    })
                    .OrderBy(x => x.Rating).First();
            var expectedDate = worstRatingEventInfo.Event.HappensDate;
            var expectedRating = worstRatingEventInfo.Rating;
            var expectedPriority = MaxRating - expectedRating;
            var expectedText = worstRatingEventInfo.Event.CustomizationsParameters.Comment.ValueUnsafe().Text;
            _eventRepository.AddRangeOfEvents(eventsList);
            var allEvents = _eventRepository.LoadAllTrackerEvents(tracker.Id);

            //act 
            var fact = new WorstRatingEventCalculator().Calculate(allEvents, tracker, _now)
                .ConvertTo<WorstRatingEventFact>().ValueUnsafe();

            //arrange 
            Assert.AreEqual("Худшее событие", fact.FactName);
            Assert.AreEqual($"Событие {tracker.Name} с самым низким рейтингом {expectedRating} " +
                            $"произошло {expectedDate:d} с комментарием {expectedText}", fact.Description);
            Assert.AreEqual(expectedPriority, fact.Priority);
            Assert.AreEqual(expectedRating, fact.WorstRating);
            Assert.AreEqual(expectedDate, fact.WorstEventDate);
            Assert.AreEqual(expectedText, fact.WorstEventComment.ValueUnsafe().Text);
        }
    }
}