using System;
using System.Collections.Generic;
using ItHappened.Api.MappingProfiles;
using ItHappened.Application.Services.StatisticService;
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
            var userId = User.GetUserId();
            var facts = _statisticsService.GetSingleTrackerFacts(trackerId, userId);
            return Ok(_myMapper.SingleFactsToJson(facts));
        }
        
        [HttpGet("statistics")]
        [ProducesResponseType(200, Type = typeof(List<ITrackerFact>))]
        public IActionResult GetStatisticsForAllTrackers()
        {
            var userId = User.GetUserId();
            var facts = _statisticsService.GetMultipleTrackersFacts(userId);
            return Ok(_myMapper.MultipleFactsToJson(facts));
        }
    }
}