using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Application.Errors;
using ItHappened.Application.Services.StatisticService;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure.Repositories;
using ItHappened.UnitTests.StatisticsCalculatorsTests;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;

namespace ItHappened.UnitTests.ServicesTests
{
    public class StatisticServiceTests
    {
        private IMultipleFactsRepository _multipleFactsRepository;
        private ISingleFactsRepository _singleFactsRepository;
        private ITrackerRepository _trackerRepository;
        private IStatisticsService _statisticsService;
        private readonly EventTracker _tracker = TestingMethods.CreateTrackerWithDefaultCustomization(Guid.NewGuid());
        private List<Event> _events;

        [SetUp]
        public void Init()
        {
            _multipleFactsRepository = new MultipleFactsRepository();
            _singleFactsRepository = new SingleFactsRepository();
            _trackerRepository = new TrackerRepository();
            _statisticsService = new StatisticsService(
                _multipleFactsRepository,
                _singleFactsRepository,
                _trackerRepository);
            _events = InitializeEvents();
        }
        
        [Test]
        public void GetSingleTrackerFactsWhenTrackerDontExistInRepository_ThrowsRestException()
        {
            Assert.Throws<BusinessException>(() => 
                _statisticsService.GetSingleTrackerFacts(_tracker.Id, _tracker.CreatorId));
        }
        
        [Test]
        public void GetSingleTrackerFactsWhenUserAsksSomeoneElseTracker_ThrowsRestException()
        {
            _trackerRepository.SaveTracker(_tracker);
            
            Assert.Throws<BusinessException>(() => 
                _statisticsService.GetSingleTrackerFacts(_tracker.Id, Guid.NewGuid()));
        }
        
        [Test]
        public void GetSingleTrackerFactsWhenNoFactsInRepositoryForTheTracker_ThrowsRestException()
        {
            _trackerRepository.SaveTracker(_tracker);
            
            Assert.Throws<BusinessException>(() => 
                _statisticsService.GetSingleTrackerFacts(_tracker.Id, _tracker.CreatorId));
        }    
        
        [Test]
        public void GetSingleTrackerFactsGoodCase_ReturnFacts()
        {
            var singleTrackerFact = CreateSpecificFact();
            _trackerRepository.SaveTracker(_tracker);
            _singleFactsRepository.UpdateTrackerSpecificFacts(_tracker.Id, new []{singleTrackerFact});

            var facts = _statisticsService.GetSingleTrackerFacts(_tracker.Id, _tracker.CreatorId);
            
            Assert.AreEqual(singleTrackerFact.GetHashCode(), facts.First().GetHashCode());
        }

        [Test]
        public void GetMultipleTrackersFactsWhenNoFactsInRepositoryForTheUser_ThrowsRestException()
        {
            Assert.Throws<BusinessException>(() => 
                _statisticsService.GetMultipleTrackersFacts(Guid.NewGuid()));
        }
        
        [Test]
        public void GetMultipleTrackersFactsGoodCase_ReturnFacts()
        {
            var multipleTrackerFact = CreateGeneralFact();
            _trackerRepository.SaveTracker(_tracker);
            _multipleFactsRepository.UpdateUserGeneralFacts(_tracker.CreatorId, new []{multipleTrackerFact});

            var facts = _statisticsService.GetMultipleTrackersFacts(_tracker.CreatorId);
            
            Assert.AreEqual(multipleTrackerFact.GetHashCode(), facts.First().GetHashCode());
        }
        
        private List<Event> InitializeEvents()
        {
            var event1 = TestingMethods.CreateEvent(_tracker.Id, _tracker.CreatorId);
            var event2 = TestingMethods.CreateEvent(_tracker.Id, _tracker.CreatorId);
            return new List<Event>() {event1, event2};
        }
        
        private IMultipleTrackersFact CreateGeneralFact()
        {
            var calculator = new MostEventfulDayCalculator();
            var fact = calculator.Calculate(new[] {new TrackerWithItsEvents(_tracker, _events)}, DateTimeOffset.Now);
            return fact.ValueUnsafe();
        }
        
        private ISingleTrackerFact CreateSpecificFact()
        {
            var calculator = new SingleTrackerEventsCountCalculator();
            var fact = calculator.Calculate(_events, _tracker, DateTimeOffset.Now);
            return fact.ValueUnsafe();
        }
    }
}