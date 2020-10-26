using System;
using System.Collections.Generic;
using System.Security.Claims;
using ItHappened.Api.Authentication;
using ItHappened.Api.MappingProfiles;
using ItHappened.Application.Services.StatisticService;
using ItHappened.Domain.Statistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ItHappened.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;
        
        private readonly IMyMapper _myMapper;
        
        public StatisticsController(IStatisticsService statisticsService, IMyMapper myMapper)
        {
            _statisticsService = statisticsService;
            _myMapper = myMapper;
        }

        [HttpGet("statistics/{trackerId}")]
        [ProducesResponseType(200, Type = typeof(ITrackerFact))]
        public IActionResult GetStatisticsForSingleTracker([FromRoute]Guid trackerId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var facts = _statisticsService.GetStatisticsFactsForTracker(trackerId, userId);
            return Ok(_myMapper.SingleFactsToJson(facts));
        }
        
        [HttpGet("statistics")]
        [ProducesResponseType(200, Type = typeof(List<ITrackerFact>))]
        public IActionResult GetStatisticsForAllTrackers()
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var facts = _statisticsService.GetStatisticsFactsForAllTrackers(userId);
            return Ok(_myMapper.MultipleFactsToJson(facts));
        }
    }
}