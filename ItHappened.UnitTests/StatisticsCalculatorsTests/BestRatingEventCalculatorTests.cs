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
            var tracker = CreateTrackerWithScale("Kg");
            var (events, ratings) =
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
            var now = DateTimeOffset.Now;
            var tracker = CreateTrackerWithScale("Kg");
            var (events, ratings) =
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
            var now = DateTimeOffset.Now;
            var tracker = CreateTrackerWithScale("Kg");
            var (events, ratings) =
                CreateEventsWithCommentAndWithRatingInsideFromToTime(tracker.Id, MinEventForCalculation,
                    now.AddMonths(-MonthsThreshold).AddDays(1), now.AddDays(-(DaysSinceBestEventThreshold+1)));
            _eventRepository.AddRangeOfEvents(events);
            var allEvents = _eventRepository.LoadAllTrackerEvents(tracker.Id);

            //act 
            var actual = new BestRatingCalculator().Calculate(allEvents, tracker)
                .ConvertTo<BestRatingCalculator>();

            //arrange 
            Assert.IsTrue(actual.IsNone);
        }
        
        [Test]
        public void EarliestEventsAfterThreeMonthButEventWithMaxRatingAfter7Days_CalculateFailure()
        {
            //assert
            var now = DateTimeOffset.Now;
            var tracker = CreateTrackerWithScale("Kg");
            var (events, ratings) =
                CreateEventsWithCommentAndWithRatingInsideFromToTime(tracker.Id, MinEventForCalculation,
                    now.AddMonths(-MonthsThreshold).AddDays(1), now);
            var eventsList = events.ToList();
            eventsList.Add(CreateEventWithRatingAndCommentInsideFromTo(tracker.Id, 0.00, new Comment("3 month edge"), now.AddDays(-DaysSinceBestEventThreshold).AddMilliseconds(1), now));
            _eventRepository.AddRangeOfEvents(eventsList);
            var allEvents = _eventRepository.LoadAllTrackerEvents(tracker.Id);

            //act 
            var actual = new BestRatingCalculator().Calculate(allEvents, tracker)
                .ConvertTo<BestRatingCalculator>();

            //arrange 
            Assert.IsTrue(actual.IsNone);
        }
    }
}