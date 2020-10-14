using System;
using System.Collections.Generic;

namespace ItHappend.Domain
{
    public class MostFrequentEventsService
    {
        private IList<EventTracker> _eventTrackers;
        private Guid _userId;

        public MostFrequentEventsService(IList<EventTracker> eventTrackers, Guid userId)
        {
            _eventTrackers = eventTrackers;
            _userId = userId;
        }

        public TheMostFrequentEvent CreateFact()
        {
            _eventTrackers = FilterByApplicabilityFunction();
            var eventsPeriodsDictionary = CountEventsPeriod();
            var requiredTracker = FindTrackerWithMostFrequentEvent();
            var eventsPeriod = eventsPeriodsDictionary[requiredTracker.Id];
            var priority = 10 / eventsPeriodsDictionary[requiredTracker.Id];
            var barChart = CreateBarChart();
            var res = new TheMostFrequentEvent(requiredTracker.Name, eventsPeriod, barChart, priority);
            return res;
        }

        private Dictionary<Guid, double> CountEventsPeriod()
        {
            foreach (var tracker in _eventTrackers)
            {
                throw new NotImplementedException();
            }
        }

        private EventTracker FindTrackerWithMostFrequentEvent()
        {
            throw new NotImplementedException();
        }

        public object CreateBarChart()
        {
            //по всем трекерам
            throw new NotImplementedException();
        }

        private IList<EventTracker> FilterByApplicabilityFunction()
        {
            throw new NotImplementedException();
        }
        
    }
}