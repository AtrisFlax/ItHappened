using System;
using System.Security.Claims;
using ItHappened.Api.Authentication;
using ItHappened.Api.Mapping;
using ItHappened.Application.Services.StatisticService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;
        private readonly IFactsToJsonMapper _mapper;

        public StatisticsController(IStatisticsService statisticsService, IFactsToJsonMapper mapper)
        {
            _statisticsService = statisticsService;
            _mapper = mapper;
        }

        [HttpGet("statistics/{trackerId}")]
        [ProducesResponseType(200)]
        public IActionResult GetStatisticsForSingleTracker([FromRoute] Guid trackerId)
        {
            var userId = User.GetUserId();
            var facts = _statisticsService.GetSingleTrackerFacts(trackerId, userId);
            return Ok(_mapper.SingleFactsToJson(facts));
        }

        [HttpGet("statistics")]
        [ProducesResponseType(200)]
        public IActionResult GetStatisticsForAllTrackers()
        {
            var userId = User.GetUserId();
            var facts = _statisticsService.GetMultipleTrackersFacts(userId);
            return Ok(_mapper.MultipleFactsToJson(facts));
        }
    }
}