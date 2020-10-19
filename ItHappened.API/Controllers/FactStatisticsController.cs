using System;
using System.Collections.Generic;
using System.Text.Json;
using AutoMapper;
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
        private IMapper _mapper;

        public FactStatisticsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{userId}/CalculateAll")]
        public IActionResult CalculateAll([FromRoute]Guid userId)
        {
            // var statisticsService = new StatisticsService(new EventTrackerRepository());
            
            //create tracker with events for test :(
            // var eventTrackerService = new EventTrackerService(new EventTrackerRepository(), new EventRepository());
            // var trackerId = eventTrackerService.CreateTracker(userId, "firstTracker");
            // eventTrackerService.AddEventToTracker
            //     (userId, trackerId, 
            //     new Event(EventBuilder.Event(Guid.NewGuid(), userId, DateTimeOffset.Now, "school")));
            // eventTrackerService.AddEventToTracker
            // (userId, trackerId, 
            //     new Event(EventBuilder.Event(Guid.NewGuid(), userId, DateTimeOffset.Now, "school1")));
            // eventTrackerService.AddEventToTracker
            // (userId, trackerId, 
            //     new Event(EventBuilder.Event(Guid.NewGuid(), userId, DateTimeOffset.Now, "school2")));
            // eventTrackerService.AddEventToTracker
            // (userId, trackerId, 
            //     new Event(EventBuilder.Event(Guid.NewGuid(), userId, DateTimeOffset.Now, "school3")));
            // eventTrackerService.AddEventToTracker
            // (userId, trackerId, 
            //     new Event(EventBuilder.Event(Guid.NewGuid(), userId, DateTimeOffset.Now, "school4")));
            // eventTrackerService.AddEventToTracker
            // (userId, trackerId, 
            //     new Event(EventBuilder.Event(Guid.NewGuid(), userId, DateTimeOffset.Now, "school")));
            // eventTrackerService.AddEventToTracker
            // (userId, trackerId, 
            //     new Event(EventBuilder.Event(Guid.NewGuid(), userId, DateTimeOffset.Now, "school1")));
            // eventTrackerService.AddEventToTracker
            // (userId, trackerId, 
            //     new Event(EventBuilder.Event(Guid.NewGuid(), userId, DateTimeOffset.Now, "school2")));
            // eventTrackerService.AddEventToTracker
            // (userId, trackerId, 
            //     new Event(EventBuilder.Event(Guid.NewGuid(), userId, DateTimeOffset.Now, "school3")));
            // eventTrackerService.AddEventToTracker
            // (userId, trackerId, 
            //     new Event(EventBuilder.Event(Guid.NewGuid(), userId, DateTimeOffset.Now, "school4")));
            // //end
            
            //var facts = statisticsService.GetFacts(userId);
            //return Ok(JsonSerializer.Serialize(facts));
            return Ok("statisticsService does not work!");
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