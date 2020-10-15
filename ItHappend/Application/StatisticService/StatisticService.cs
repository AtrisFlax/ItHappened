using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItHappend.Domain;
using Newtonsoft.Json;

namespace ItHappend.StatisticService
{
    public class StatisticService
    {
        private readonly IStatisticEngine _statisticEngine;
        private readonly IUserRepository _userRepository;
        private readonly IEventTrackerRepository _eventTrackerRepository;
        private readonly IJsonService _jsonService;


        private StatisticService(IStatisticEngine statisticEngine, IUserRepository userRepository,
            IEventTrackerRepository eventTrackerRepository, IJsonService jsonService, Guid userId)
        {
            _statisticEngine = statisticEngine;
            _userRepository = userRepository;
            _eventTrackerRepository = eventTrackerRepository;
            _jsonService = jsonService;
        }


        //TODO Correct assembly of the log #issue 49
        public string CreateJsonReport(IStatisticEngine statisticEngine, IUserRepository userRepository,
            IEventTrackerRepository eventTrackerRepository, IJsonService jsonService, Guid userId,
            IEnumerable<Guid> targetEventIds)
        {
            var result = new StringBuilder();
            var trackers = eventTrackerRepository.LoadUserTrackers(userId);
            foreach (var eventId in targetEventIds)
            {
                try
                {
                    var targetTracker = trackers.First(tracker => tracker.Id == eventId);
                    var averageRatingFact = statisticEngine.CreateAverageRatingFact(targetTracker);
                    if (averageRatingFact.HasValue)
                    {
                        var jsonString = jsonService.SerializeObject(statisticEngine);
                        result.Append(jsonString);
                    }
                    else
                    {
                        //TODO log info for {targetTracker} we cant create {averageRatingFact.Type}
                    }
                }
                catch (InvalidOperationException exception)
                {
                    //TODO log warning invalid input eventId
                }
            }
            return result.ToString();
        }
    }
}