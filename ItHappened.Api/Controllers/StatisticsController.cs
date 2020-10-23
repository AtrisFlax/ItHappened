using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using ItHappened.Api.Authentication;
using ItHappened.Application.Services.StatisticService;
using ItHappened.Application.Services.TrackerService;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;
        private readonly ITrackerService _trackerService;
        private readonly ITrackerRepository _trackerRepository;
        private readonly IMapper _mapper;
        
        public StatisticsController(ITrackerService trackerService,
            ITrackerRepository trackerRepository,
            IStatisticsService statisticsService,
            IMapper mapper)
        {
            _trackerService = trackerService;
            _trackerRepository = trackerRepository;
            _statisticsService = statisticsService;
            _mapper = mapper;
        }

        [HttpGet("statistics/{trackerId}")]
        [ProducesResponseType(200, Type = typeof(ITrackerFact))]
        public IActionResult GetStatisticsForSingleTracker([FromRoute]Guid trackerId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var statistics = _statisticsService.GetStatisticsFactsForTracker(trackerId, userId);
            return Ok(statistics);
        }
        
        [HttpGet("statistics")]
        [ProducesResponseType(200, Type = typeof(List<ITrackerFact>))]
        public IActionResult GetStatisticsForAllTrackers()
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var statistics = _statisticsService.GetStatisticsFactsForAllTrackers(userId);
            return Ok(statistics);
        }
    }
}