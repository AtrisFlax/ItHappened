using System;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure.Repositories;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.TestingMethods;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.StatisticsCalculatorsTestingConstants;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class BestRatingEventCalculatorTests
    {
        private const int MinEventForCalculation = 11;
        private const int MonthsThreshold = 3;
        private const int DaysSinceBestEventThreshold = 7;
        private IEventRepository _eventRepository;
        private DateTimeOffset _now;


        [SetUp]
        public void Init()
        {
            _eventRepository = new EventRepository();
            _now = DateTimeOffset.UtcNow;
        }

        [Test]
        public void LessThanRequiredNumberEvents_CalculateFailure()
        {
            //assert
            var userId = Guid.NewGuid();
            var tracker = CreateTrackerWithScale(userId, "Kg");
            var (events, _, _) =
                CreateEventsWithCommentAndWithRating(tracker.Id, userId, MinEventForCalculation - 1);
            _eventRepository.AddRangeOfEvents(events);
            var allEvents = _eventRepository.LoadAllTrackerEvents(tracker.Id);

            //act
            var fact = new BestRatingEventCalculator().Calculate(allEvents, tracker, _now)
                .ConvertTo<BestRatingEventCalculator>();

            //arrange
            Assert.IsTrue(fact.IsNone);
        }


        [Test]
        public void AllEventsAfter3Months_CalculateFailure()
        {
            //assert
            var userId = Guid.NewGuid();
            var now = DateTimeOffset.Now;
            var tracker = CreateTrackerWithScale(userId, "Kg");
            var (events, _) =
                CreateEventsWithCommentAndWithRatingInsideFromToTime(tracker.Id, userId, MinEventForCalculation,
                    now.AddMonths(-3), now.AddMonths(-MonthsThreshold));
            _eventRepository.AddRangeOfEvents(events);
            var allEvents = _eventRepository.LoadAllTrackerEvents(tracker.Id);

            //act 
            var actual = new BestRatingEventCalculator().Calculate(allEvents, tracker, _now)
                .ConvertTo<BestRatingEventFact>();

            //arrange 
            Assert.IsTrue(actual.IsNone);
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
                    now.AddDays(-DaysSinceBestEventThreshold));
            var eventsList = events.ToList();
            var eventBefore3Month = CreateEventWithRatingWithCommentAndFixDate(tracker.Id, userId, MinRatingValue,
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
                    .OrderByDescending(x => x.Rating).First();
            var expectedDate = worstRatingEventInfo.Event.HappensDate;
            var expectedRating = worstRatingEventInfo.Rating;
            var expectedPriority = expectedRating;
            var expectedText = worstRatingEventInfo.Event.CustomizationsParameters.Comment.ValueUnsafe().Text;
            _eventRepository.AddRangeOfEvents(eventsList);
            var allEvents = _eventRepository.LoadAllTrackerEvents(tracker.Id);

            //act 
            var fact = new BestRatingEventCalculator().Calculate(allEvents, tracker, _now)
                .ConvertTo<BestRatingEventFact>().ValueUnsafe();

            //arrange 
            Assert.AreEqual("Лучшее событие", fact.FactName);
            Assert.AreEqual($"Событие {tracker.Name} с самым высоким рейтингом {expectedRating} " +
                            $"произошло {expectedDate:d} с комментарием {expectedText}", fact.Description);
            Assert.AreEqual(expectedPriority, fact.Priority);
            Assert.AreEqual(expectedRating, fact.BestRating);
            Assert.AreEqual(expectedDate, fact.BestEventDate);
            Assert.AreEqual(expectedText, fact.BestEventComment.ValueUnsafe().Text);
        }
    }
}