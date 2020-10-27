using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure.Repositories;
using ItHappened.UnitTests.StatisticsCalculatorsTests;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;

namespace ItHappened.UnitTests.StatisticsProvidersTests
{
    public class StatisticGeneratorTests
    {
        private IMultipleFactsRepository _multipleFactsRepository;
        private ISingleFactsRepository _singleFactsRepository;
        private readonly EventTracker _tracker = TestingMethods.CreateTracker(Guid.NewGuid());
        private List<Event> _events;
        private IEventRepository _eventRepository;
        private StatisticGenerator _statisticGenerator;
        private ITrackerRepository _trackerRepository;

        [SetUp]
        public void Init()
        {
            _statisticGenerator = InitializeStatisticGeneratorWithTwoCalculators();
            _events = InitializeEvents();
            _eventRepository.AddRangeOfEvents(_events);
            _trackerRepository.SaveTracker(_tracker);
        }
        
        [Test]
        public void UpdateUserFactsWhenUserHasNoTrackers_UserHasNoFactsInRepos()
        {
            var userId = Guid.NewGuid();
            _statisticGenerator.UpdateUserFacts(userId);

            Assert.False(_multipleFactsRepository.IsContainFactsForUser(userId));
        }

        private void PutInitialFactsInFactsRepositories()
        {
            
        }
        
        [Test]
        public void UpdateUserFactsWhenUserHasNoUpdatedTrackers_FactsInRepositoryNotUpdated()
        {
            var generalFact = CreateGeneralFact();
            _multipleFactsRepository.UpdateUserGeneralFacts(_tracker.CreatorId, new []{generalFact});
            var specificFact = CreateSpecificFact();
            _singleFactsRepository.UpdateTrackerSpecificFacts(_tracker.Id, new []{specificFact});
            _tracker.IsUpdated = false;
            _trackerRepository.UpdateTracker(_tracker);
            
            _statisticGenerator.UpdateUserFacts(_tracker.CreatorId);
            var loadedGeneralFact = _multipleFactsRepository.LoadUserGeneralFacts(_tracker.CreatorId);
            var loadedSpecificFact = _singleFactsRepository.LoadTrackerSpecificFacts(_tracker.Id);
            
            Assert.AreEqual(generalFact.GetHashCode(), loadedGeneralFact.First().GetHashCode());
            Assert.AreEqual(specificFact.GetHashCode(), loadedSpecificFact.First().GetHashCode());
        }
        
        [Test]
        public void UpdateUserFactsWhenTrackerWasUpdated_FactsInRepositoryUpdated()
        {
            var newEvent = TestingMethods.CreateEventFixDate(_tracker.Id, _tracker.CreatorId, DateTimeOffset.Now - TimeSpan.FromDays(2));
            var newEvent2 = TestingMethods.CreateEventFixDate(_tracker.Id, _tracker.CreatorId, DateTimeOffset.Now - TimeSpan.FromDays(2));
            _eventRepository.AddRangeOfEvents(new []{newEvent, newEvent2});
            var generalFact = CreateGeneralFact();
            _multipleFactsRepository.UpdateUserGeneralFacts(_tracker.CreatorId, new []{generalFact});
            var specificFact = CreateSpecificFact();
            _singleFactsRepository.UpdateTrackerSpecificFacts(_tracker.Id, new []{specificFact});
            _tracker.IsUpdated = true;
            _trackerRepository.UpdateTracker(_tracker);
            
            _statisticGenerator.UpdateUserFacts(_tracker.CreatorId);
            var loadedGeneralFact = _multipleFactsRepository.LoadUserGeneralFacts(_tracker.CreatorId);
            var loadedSpecificFact = _singleFactsRepository.LoadTrackerSpecificFacts(_tracker.Id);
            
            Assert.AreNotEqual(generalFact.GetHashCode(), loadedGeneralFact.First().GetHashCode());
            Assert.AreNotEqual(specificFact.GetHashCode(), loadedSpecificFact.First().GetHashCode());
        }

        private List<Event> InitializeEvents()
        {
            var event1 = TestingMethods.CreateEvent(_tracker.Id, _tracker.CreatorId);
            var event2 = TestingMethods.CreateEvent(_tracker.Id, _tracker.CreatorId);
            return new List<Event>() {event1, event2};
        }
        private IMultipleTrackersFact CreateGeneralFact()
        {
            var calculator = new MostEventfulDayStatisticsCalculator();
            var fact = calculator.Calculate(new[] {new TrackerWithItsEvents(_tracker, _events)});
            return fact.ValueUnsafe();
        }
        
        private ISingleTrackerFact CreateSpecificFact()
        {
            var calculator = new SingleTrackerEventsCountCalculator();
            var fact = calculator.Calculate(_events, _tracker);
            return fact.ValueUnsafe();
        }
        
        private StatisticGenerator InitializeStatisticGeneratorWithTwoCalculators()
        {
            var generalFactProvider = new MultipleTrackersFactProvider();
            generalFactProvider.Add(new MostEventfulDayStatisticsCalculator());
            var specificFactProvider = new SingleTrackerFactProvider();
            specificFactProvider.Add(new SingleTrackerEventsCountCalculator());
            
            _singleFactsRepository = new SingleFactsRepository();
            _multipleFactsRepository = new MultipleFactsRepository();
            _trackerRepository = new TrackerRepository();
            _eventRepository = new EventRepository();
            return new StatisticGenerator(_multipleFactsRepository,
                generalFactProvider,
                specificFactProvider,
                _singleFactsRepository,
                _trackerRepository,
                _eventRepository);
        }
    }
}