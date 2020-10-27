using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.UnitTests.StatisticsCalculatorsTests;
using NUnit.Framework;

namespace ItHappened.UnitTests.StatisticsProvidersTests
{
    public class MultipleTrackerFactProviderTests
    {
        private List<IMultipleTrackersStatisticsCalculator> _calculators;
        private IMultipleTrackersFactProvider _factProvider;

        [Test]
        public void GetFactsFromTrackerWithEvents_ReturnsFacts()
        {
            InitializeFactProviderWithCalculators();
            var tracker = TestingMethods.CreateTracker(Guid.NewGuid());
            var event1 = TestingMethods.CreateEvent(tracker.Id, tracker.CreatorId);
            var event2 = TestingMethods.CreateEvent(tracker.Id, tracker.CreatorId);
            const int expected = 2;
            
            var facts = _factProvider
                .GetFacts(new [] {new TrackerWithItsEvents(tracker, new []{event1, event2})});
            var firstFact = facts.First();
            var secondFact = facts.Last();
            
            Assert.AreEqual(expected, facts.Count);
            Assert.AreEqual("Самая насыщенная событиями неделя", firstFact.FactName);
            Assert.AreEqual("Самый насыщенный событиями день", secondFact.FactName);
        }

        [Test]
        public void GetFactsFromTrackerWithoutEvents_ReturnsEmptyFactsCollection()
        {
            InitializeFactProviderWithCalculators();
            var tracker = TestingMethods.CreateTracker(Guid.NewGuid());
            var emtyEventsCollection = new List<Event>();
            const int expected = 0;
            
            var facts = _factProvider
                .GetFacts(new [] {new TrackerWithItsEvents(tracker, emtyEventsCollection)});

            Assert.AreEqual(expected, facts.Count);
        }

        
        private void InitializeFactProviderWithCalculators()
        {
            _factProvider = new MultipleTrackersFactProvider();
            _calculators = CreateCalculators();
            foreach (var calculator in _calculators)
            {
                _factProvider.Add(calculator);
            }
        }
        
        private List<IMultipleTrackersStatisticsCalculator> CreateCalculators()
        {
            return new List<IMultipleTrackersStatisticsCalculator>()
            {
                new MostEventfulWeekCalculator(),
                new MostEventfulDayStatisticsCalculator(),
                new MostFrequentEventStatisticsCalculator(),
                new MultipleTrackersEventsCountCalculator()
            };
        }
    }
}