using System;
using System.Collections.Generic;
using ItHappened.Application.Errors;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.Application.Services.StatisticService
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IMultipleFactsRepository _multipleFactsRepository;
        private readonly ISingleFactsRepository _singleFactsRepository;
        private readonly ITrackerRepository _trackerRepository;

        public StatisticsService(IMultipleFactsRepository multipleFactsRepository,
            ISingleFactsRepository singleFactsRepository, ITrackerRepository trackerRepository)
        {
            _multipleFactsRepository = multipleFactsRepository;
            _singleFactsRepository = singleFactsRepository;
            _trackerRepository = trackerRepository;
        }

        [Produces("application/json")]
        public IReadOnlyCollection<IMultipleTrackersFact> GetMultipleTrackersFacts(Guid userId)
        {
            if (!_multipleFactsRepository.IsContainFactsForUser(userId))
            {
                throw new UserTrackersStatisticsNotFoundException(userId);
            }

            var statisticFacts = _multipleFactsRepository.ReadUserGeneralFacts(userId);
            return statisticFacts;
        }

        [Produces("application/json")]
        public IReadOnlyCollection<ISingleTrackerFact> GetSingleTrackerFacts(Guid trackerId, Guid userId)
        {
            if (!_trackerRepository.IsContainTracker(trackerId))
            {
                throw new TrackerNotFoundException(trackerId);
            }

            var tracker = _trackerRepository.LoadTracker(trackerId);
            if (userId != tracker.CreatorId)
            {
                throw new NoPermissionsForTrackerException(userId, trackerId);
            }

            if (!_singleFactsRepository.IsContainFactForTracker(trackerId, userId))
            {
                throw new TrackerStatisticsNotFoundException(trackerId);
            }

            var statisticFacts = _singleFactsRepository.ReadTrackerSpecificFacts(userId, trackerId);
            return statisticFacts;
        }
    }
}