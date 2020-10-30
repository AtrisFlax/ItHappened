using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using ItHappened.Api.Authentication;
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
        private readonly IMapper _mapper;

        public StatisticsController(IStatisticsService statisticsService, IMapper mapper)
        {
            _statisticsService = statisticsService;
            _mapper = mapper;
        }

        [HttpGet("statistics/{trackerId}")]
        [ProducesResponseType(200, Type = typeof(ITrackerFact))]
        public IActionResult GetStatisticsForSingleTracker([FromRoute] Guid trackerId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id)); //TODO issue  #169
            // var facts = _statisticsService.GetSingleTrackerFacts(trackerId, userId);

            var facts = new ISingleTrackerFact[]
            {
                new AverageScaleTrackerFact("Scale", "description1", 1.0, 1.1, "unit1"),
                new AverageRatingTrackerFact("Rating", "description2", 2.1, 2.2)
            };
            return Ok(_mapper.Map<ISingleTrackerFact>(facts));
        }

        [HttpGet("statistics")]
        [ProducesResponseType(200, Type = typeof(List<ITrackerFact>))]
        public IActionResult GetStatisticsForAllTrackers()
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id)); //TODO issue  #169
            var facts = _statisticsService.GetMultipleTrackersFacts(userId);
            return Ok(_mapper.Map<IMultipleTrackersFact>(facts));
        }
    }
}