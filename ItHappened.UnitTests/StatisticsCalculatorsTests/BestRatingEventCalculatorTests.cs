using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure.Repositories;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.TestingMethods;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class BestRatingEventCalculatorTests
    {
        private const int MinEventForCalculation = 11;
        private const int MonthsThreshold = 3;
        private const int DaysSinceBestEventThreshold = 7;
        private IEventRepository _eventRepository;


        [SetUp]
        public void Init()
        {
            _eventRepository = new EventRepository();
        }

        [Test]
        public void LessThanRequiredNumberEvents_CalculateFailure()
        {
            //assert
            var userId = Guid.NewGuid();
            var tracker = CreateTrackerWithScale(userId,"Kg");
            var (events, _) =
                CreateEventsWithCommentAndWithRating(tracker.Id, MinEventForCalculation - 1);
            _eventRepository.AddRangeOfEvents(events);
            var allEvents = _eventRepository.LoadAllTrackerEvents(tracker.Id);

            //act
            var fact = new BestRatingCalculator().Calculate(allEvents, tracker)
                .ConvertTo<BestRatingCalculator>();

            //arrange
            Assert.IsTrue(fact.IsNone);
        }


        [Test]
        public void EventsTooOld_CalculateFailure()
        {
            //assert
            var userId = Guid.NewGuid();
            var now = DateTimeOffset.Now;
            var tracker = CreateTrackerWithScale(userId, "Kg");
            var (events, _) =
                CreateEventsWithCommentAndWithRatingInsideFromToTime(tracker.Id, MinEventForCalculation,
                    now.AddMonths(-6), now.AddMonths(-MonthsThreshold));
            _eventRepository.AddRangeOfEvents(events);
            var allEvents = _eventRepository.LoadAllTrackerEvents(tracker.Id);

            //act 
            var actual = new BestRatingCalculator().Calculate(allEvents, tracker)
                .ConvertTo<BestRatingCalculator>();

            //arrange 
            Assert.IsTrue(actual.IsNone);
        }


        [Test]
        public void EarliestEventsAfterThreeMonthButEventWithMaxRatingTooOld_CalculateFailure()
        {
            //assert
            var userId = Guid.NewGuid();
            var now = DateTimeOffset.Now;
            var tracker = CreateTrackerWithScale(userId,"Kg");
            var (events, _) =
                CreateEventsWithCommentAndWithRatingInsideFromToTime(tracker.Id, MinEventForCalculation,
                    now.AddMonths(-MonthsThreshold).AddDays(1), now.AddDays(-(DaysSinceBestEventThreshold + 1)));
            _eventRepository.AddRangeOfEvents(events);
            var allEvents = _eventRepository.LoadAllTrackerEvents(tracker.Id);

            //act 
            var actual = new BestRatingCalculator().Calculate(allEvents, tracker)
                .ConvertTo<BestRatingCalculator>();

            //arrange 
            Assert.IsTrue(actual.IsNone);
        }

        [Repeat(1000)]
        [Test]
        public void EarliestEventsAfterThreeMonthOneEventWithMaxRatingAfter7Days_CalculateSuccess()
        {
            //assert
            var userId = Guid.NewGuid();
            var now = DateTimeOffset.Now;
            var tracker = CreateTrackerWithScale(userId,"Kg");
            var (events, _) =
                CreateEventsWithCommentAndWithRatingInsideFromToTime(tracker.Id, MinEventForCalculation,
                    now.AddMonths(-MonthsThreshold).AddDays(1), now.AddDays(-DaysSinceBestEventThreshold).AddTicks(-1));
            var eventsList = events.ToList();
            const double bestRating = 1.00;
            const double expectedPriority = bestRating;
            const string commentText = "7 day edge + delta";
            var fromWeekAgo = now.AddDays(-DaysSinceBestEventThreshold).AddMinutes(1);
            var eventFromToLastWeek =
                CreateEventWithRatingAndCommentInsideFromTo(tracker.Id, bestRating, new Comment(commentText),
                    fromWeekAgo,
                    now);
            var expectedDate = eventFromToLastWeek.HappensDate;
            eventsList.Add(eventFromToLastWeek);
            _eventRepository.AddRangeOfEvents(eventsList);
            var allEvents = _eventRepository.LoadAllTrackerEvents(tracker.Id);

            //act 
            var fact = new BestRatingCalculator().Calculate(allEvents, tracker)
                .ConvertTo<BestEventTrackerFact>().ValueUnsafe();

            //arrange 
            Assert.AreEqual("Лучшее событие", fact.FactName);
            Assert.AreEqual($"Событие {tracker.Name} с самым высоким рейтингом {bestRating} " +
                            $"произошло {expectedDate:d} с комментарием {commentText}", fact.Description);
            Assert.AreEqual(expectedPriority, fact.Priority);
            Assert.AreEqual(bestRating, fact.BestRating);
            Assert.AreEqual(expectedDate, fact.BestEventDate);
            Assert.AreEqual(commentText, fact.BestEventComment.ValueUnsafe().Text);
        }

        [Repeat(1000)]
        [Test]
        public void EarliestEventsAfterThreeMonthTwoEventWithMaxRatingAfter7Days_CalculateSuccess()
        {
            //assert
            var userId = Guid.NewGuid();
            var now = DateTimeOffset.Now;
            var tracker = CreateTrackerWithScale(userId,"Kg");
            var (events, _) =
                CreateEventsWithCommentAndWithRatingInsideFromToTime(tracker.Id, MinEventForCalculation,
                    now.AddMonths(-MonthsThreshold).AddDays(1), now.AddDays(-DaysSinceBestEventThreshold).AddTicks(-1));
            var eventsList = events.ToList();
            const double bestRating = 2.00;
            const double expectedPriority = bestRating;
            const string commentText = "7 day edge + delta";
            var fromWeekAgo = now.AddDays(-DaysSinceBestEventThreshold).AddMinutes(1);
            var eventsFromToLastWeek = new List<Event>
            {
                CreateEventWithRatingAndCommentInsideFromTo(tracker.Id, bestRating - 1.0, new Comment(commentText),
                    fromWeekAgo,
                    now),
                CreateEventWithRatingAndCommentInsideFromTo(tracker.Id, bestRating, new Comment(commentText),
                    fromWeekAgo,
                    now),
            };
            var expectedDate = eventsFromToLastWeek[1].HappensDate;
            eventsList.AddRange(eventsFromToLastWeek);
            _eventRepository.AddRangeOfEvents(eventsList);
            var allEvents = _eventRepository.LoadAllTrackerEvents(tracker.Id);

            //act 
            var fact = new BestRatingCalculator().Calculate(allEvents, tracker)
                .ConvertTo<BestEventTrackerFact>().ValueUnsafe();

            //arrange 
            Assert.AreEqual("Лучшее событие", fact.FactName);
            Assert.AreEqual($"Событие {tracker.Name} с самым высоким рейтингом {bestRating} " +
                            $"произошло {expectedDate:d} с комментарием {commentText}", fact.Description);
            Assert.AreEqual(expectedPriority, fact.Priority);
            Assert.AreEqual(bestRating, fact.BestRating);
            Assert.AreEqual(expectedDate, fact.BestEventDate);
            Assert.AreEqual(commentText, fact.BestEventComment.ValueUnsafe().Text);
        }
    }
}