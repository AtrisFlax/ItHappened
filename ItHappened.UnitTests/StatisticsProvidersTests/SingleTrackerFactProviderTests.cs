﻿using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.UnitTests.StatisticsCalculatorsTests;
using LanguageExt;
using NUnit.Framework;

namespace ItHappened.UnitTests.StatisticsProvidersTests
{
    public class SingleTrackerFactProviderTests
    {
        private List<ISingleTrackerStatisticsCalculator> _calculators;
        private ISingleTrackerFactProvider _factProvider;

        [Test]
        public void GetFacts_ReturnsFacts()
        {
            InitializeFactProviderWithCalculators();
            var tracker = CreateTrackerWithRating();
            var event1 = TestingMethods.CreateEventWithRating(tracker.Id, tracker.CreatorId, 2);
            var event2 = TestingMethods.CreateEventWithRating(tracker.Id, tracker.CreatorId, 4);
            const int expected = 2;
            
            var facts = _factProvider.GetFacts(new[] {event1, event2}, tracker);
            var firstFact = facts.First();
            var secondFact = facts.Last();
            
            Assert.AreEqual(expected, facts.Count);
            Assert.AreEqual("Среднее значение оценки", firstFact.FactName);
            Assert.AreEqual("Количество событий", secondFact.FactName);
        }

        private void InitializeFactProviderWithCalculators()
        {
            _factProvider = new SingleTrackerFactProvider();
            _calculators = CreateCalculators();
            foreach (var calculator in _calculators)
            {
                _factProvider.Add(calculator);
            }
        }
        
        private List<ISingleTrackerStatisticsCalculator> CreateCalculators()
        {
            return new List<ISingleTrackerStatisticsCalculator>()
            {
                new AverageRatingCalculator(),
                new BestRatingEventCalculator(),
                new LongestBreakCalculator(),
                new OccursOnCertainDaysOfTheWeekCalculator(),
                new SingleTrackerEventsCountCalculator(),
                new SumScaleCalculator(),
                new WorstEventCalculator()
            };
        }
        
        public EventTracker CreateTrackerWithRating()
        {
            return new EventTracker(Guid.NewGuid(), Guid.NewGuid(), "Tracker name",
                new TrackerCustomizationSettings(Option<string>.None, 
                    false,
                    true,
                    false,
                    false,
                    false,
                    false));
        }
    }
}