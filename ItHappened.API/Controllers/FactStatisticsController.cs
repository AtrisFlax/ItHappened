using System;
using System.Collections.Generic;
using System.Text.Json;
using ItHappened.Application.Services.EventTrackerService;
using ItHappened.Application.Services.StatisticService;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.API.Controllers
{
    [Route("user")]
    public class FactStatisticsController : ControllerBase
    {

        [HttpGet]
        [Route("{userId}/CalculateAll")]
        public IActionResult CalculateAll([FromRoute] Guid userId)
        {
            var statisticsService = new StatisticsService(new EventTrackerRepository(), new GeneralFactProvider(), new SpecificFactProvider());

            var eventTrackerService = new EventTrackerService(new EventTrackerRepository(), new EventRepository());
            var trackerId = eventTrackerService.CreateTracker(EventTracker.Create(Guid.NewGuid(), userId, "first tarcker").WithComment().Build());

            for (var i = 0; i < 10; i++)
                eventTrackerService.AddEventToTracker(userId, trackerId, Event.Create(Guid.NewGuid(), trackerId, userId, DateTimeOffset.Now, $"EmptyName{i}").Build());

            var facts = statisticsService.GetGeneralTrackersFacts(userId);
            return Ok(JsonSerializer.Serialize(facts));

        }

        [HttpGet]
        [Route("{userId}/Calculate/{factStatisticsId}")]
        public IActionResult Calculate([FromRoute] Guid userId, [FromRoute] Guid factStatisticsId)
        {
            // var statisticsService = new StatisticsService(new EventTrackerRepository());
            //no implementation with one fact :(
            //var fact = statisticsService.GetFacts(userId, factStatisticsId);
            throw new NotImplementedException();
        }
    }
}