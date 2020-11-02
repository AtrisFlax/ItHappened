﻿using System;
using System.Collections.Generic;
using System.Net;
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
                throw new RestException(HttpStatusCode.NotFound);
            }
            var statisticFacts = _multipleFactsRepository.ReadUserGeneralFacts(userId);
            return statisticFacts;
        }
        
        [Produces("application/json")]
        public IReadOnlyCollection<ISingleTrackerFact> GetSingleTrackerFacts(Guid trackerId, Guid userId)
        {
            if (!_trackerRepository.IsContainTracker(trackerId))
            {
                throw new RestException(HttpStatusCode.NotFound);
            }
            var tracker = _trackerRepository.LoadTracker(trackerId);
            if (userId != tracker.CreatorId)
            {
                throw new RestException(HttpStatusCode.BadRequest);
            }
            if (!_singleFactsRepository.IsContainFactForTracker(trackerId, userId))
            {
                throw new RestException(HttpStatusCode.NotFound);
            }
            var statisticFacts = _singleFactsRepository.ReadTrackerSpecificFacts(userId, trackerId);
            return statisticFacts;
        }
    }
}