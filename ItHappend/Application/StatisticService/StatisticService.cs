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
            IEventTrackerRepository eventTrackerRepository, IJsonService jsonService)
        {
            _statisticEngine = statisticEngine;
            _userRepository = userRepository;
            _eventTrackerRepository = eventTrackerRepository;
            _jsonService = jsonService;
        }


        //TODO Correct assembly of the log #issue 49
        public string CreateJsonReport(Guid userId, IEnumerable<Guid> targetEventIds)
        {
            var result = new StringBuilder();
            var trackers = _eventTrackerRepository.LoadUserTrackers(userId);
            //1. Сначала определяем значения общих фактов
            //Общие факты вычисляются для всей коллекции трекеров
            var mostCommonEvent = _statisticEngine.GetMostFrequentEventFact(trackers);
            var nEventsHappened = _statisticEngine.GetOverallEventsNumber(trackers);
            var dayWithLargestEventsCount = _statisticEngine.GetDatWithLargestEventNumber(trackers);
            var weekWithLargestEventCount = _statisticEngine.GetWeekWithLargestEventNumber(trackers);
            //Тут надо перепроверить как их объединять лучше
            result.AppendJoin(',', new [] {mostCommonEvent, nEventsHappened, dayWithLargestEventsCount, weekWithLargestEventCount});

            //2. Определяем значения специфичных фактов
            foreach (var tracker in trackers)
            {
                //Тут для каждого трекера находим всевозможные специфичные факты, если для трекера факт не подходит,
                //то нужно возвращать status code или null
                var eventsNumberFact = _statisticEngine.GetNumberOfTrackerEvents(tracker);
                var averageRatingFact = _statisticEngine.CreateAverageRatingFact(tracker);
                var averageScaleFact = _statisticEngine.GetAverageScaleFact(tracker);
                var sumOfScaleValuesFact = _statisticEngine.GetSumOfScaleValues(tracker);
                var eventDidNotHappenLongTimeFact = _statisticEngine.GetDidNotHappenLongTimeEvent(tracker);
                var eventHappensOnCertainWeekDaysFact = _statisticEngine.GetHappensOnCertainWeekDaysEvent(tracker);
                var eventHappensAtCertainTimeOfDayFact = _statisticEngine.GetHappensAtCertainTimeOfDayEvent(tracker);
                var worstEventFact = _statisticEngine.GetWorstEvent(tracker);
                var bestEventFact = _statisticEngine.GetBestEvent(tracker);
                var longestBreakFact = _statisticEngine.GetLongestBreak(tracker);
            }
            
            foreach (var eventId in targetEventIds)
            {
                try
                {
                    var targetTracker = trackers.First(tracker => tracker.Id == eventId);
                    var averageRatingFact = _statisticEngine.CreateAverageRatingFact(targetTracker);
                    if (averageRatingFact.HasValue)
                    {
                        var jsonString = _jsonService.SerializeObject(_statisticEngine);
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