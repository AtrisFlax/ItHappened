using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using LanguageExt;
using Serilog;
using Status = ItHappened.Application.Services.EventTrackerService.EventTrackerServiceStatusCodes;

namespace ItHappened.Application.Services.EventTrackerService
{
    public class EventTrackerService : IEventTrackerService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventTrackerRepository _eventTrackerRepository;

        public EventTrackerService(IEventTrackerRepository eventTrackerRepository, IEventRepository eventRepository)
        {
            _eventTrackerRepository = eventTrackerRepository;
            _eventRepository = eventRepository;
        }

        public Status DeleteTracker(Guid trackerId, Guid creatorId)
        {
            if (!_eventTrackerRepository.IsTrackerIn(trackerId))
            {
                Log.Information(
                    $"Tracker with id: {trackerId} doesn't exist in repository");
                return Status.TrackerDontExist;
            }
            var tracker = _eventTrackerRepository.LoadTracker(trackerId);
            if (creatorId != tracker.CreatorId)
            {
                Log.Information(
                    $"Can't remove tracker with trackerId={trackerId} userId={creatorId}. TrackerCreatorId does not match");
                return Status.WrongTrackerCreatorId;
            }

            Log.Information($"Tracker deleted with trackerId={trackerId} userId={creatorId}");
            return Status.Ok;;
        }

        public Guid CreateTracker(
            Guid creatorId,
            string trackerName,
            bool hasPhoto,
            bool hasScale,
            string scaleMeasurementUnit,
            bool hasRating,
            bool hashGeoTag,
            bool hasComment)
        {
            var trackerBuilder = EventTrackerBuilder
                .Tracker(creatorId, Guid.NewGuid(), trackerName);
            if (hasPhoto)
            {
                trackerBuilder = trackerBuilder.WithPhoto();
            }

            if (hasScale)
            {
                trackerBuilder = trackerBuilder.WithScale(scaleMeasurementUnit);
            }

            if (hasRating)
            {
                trackerBuilder = trackerBuilder.WithRating();
            }

            if (hashGeoTag)
            {
                trackerBuilder = trackerBuilder.WithGeoTag();
            }

            if (hasComment)
            {
                trackerBuilder = trackerBuilder.WithComment();
            }

            var tracker = trackerBuilder.Build();
            _eventTrackerRepository.SaveTracker(tracker);
            return tracker.Id;
        }

        public IReadOnlyCollection<EventTracker> GetAllUserTrackers(Guid userId)
        {
            return _eventTrackerRepository.LoadAllUserTrackers(userId).ToList();
        }
        
        
        //TODO: обсудить сценарии, в которых это может понадобиться
        // public Option<EventTracker> GetTracker(Guid trackerId, Guid creatorId )
        // {
        //     var tracker = _eventTrackerRepository.LoadTracker(trackerId);
        //     if (creatorId == tracker.CreatorId)
        //     {
        //         Log.Information(
        //             $"Can't return tracker with trackerId={trackerId} userId={creatorId}. TrackerCreatorId does not match");
        //         return Option<EventTracker>.Some(tracker);
        //     }
        //
        //     Log.Information($"Getting tracker with trackerId={trackerId} userId={creatorId}");
        //     return Option<EventTracker>.None;
        // }
    
        public Status AddEventToTracker(Guid creatorId, Guid trackerId, Event @event)
        {
            if (!_eventTrackerRepository.IsTrackerIn(trackerId))
            {
                Log.Information(
                    $"Tracker with id: {trackerId} doesn't exist in repository");
                return Status.TrackerDontExist;
            }
            
            var tracker = _eventTrackerRepository.LoadTracker(trackerId);
            if (creatorId != tracker.CreatorId)
            {
                Log.Information(
                    $"Can't add event to tracker id: {trackerId}. TrackerCreatorId does not match creator id: {creatorId}");
                return Status.WrongTrackerCreatorId;
            }

            if (tracker.CreatorId != @event.CreatorId)
            {
                Log.Information(
                    $"Can't add event {@event.Id} to tracker id: {trackerId}. Event creator id does not match tracker creator id");
                return Status.WrongEventCreatorId;
            }

            _eventRepository.AddEvent(@event);
            Log.Information(
                $"Event: {@event.Id} added to tracker: {trackerId}");
            return Status.Ok;
        }

        public bool RemoveEventFromTracker(Guid trackerCreatorId, Guid trackerId, Guid eventId)
        {
            var tracker = _eventTrackerRepository.LoadTracker(trackerId);
            if (trackerCreatorId != tracker.CreatorId)
            {
                Log.Information(
                    $"Can't remove tracker with trackerId={trackerId} eventId={eventId} userId={trackerCreatorId}. TrackerCreatorId does not match");
                return false;
            }

            var eventToRemove = _eventRepository.LoadEvent(eventId);
           //tracker.RemoveEvent(eventToRemove);
            _eventRepository.DeleteEvent(eventId);
            _eventTrackerRepository.SaveTracker(tracker);
            Log.Information(
                $"Event from tracker has deleted.  trackerId={trackerId}  eventId={eventId} userId={trackerCreatorId}");
            return true;
        }

        public bool EditEventInTracker(Guid trackerCreatorId, Guid trackerId, Guid eventId, Event newEvent)
        {
            var tracker = _eventTrackerRepository.LoadTracker(trackerId);
            if (trackerCreatorId != tracker.CreatorId)
            {
                Log.Information(
                    $"Can't edit event in tracker trackerId={trackerId} userId={trackerCreatorId}. TrackerCreatorId does not match");
                return false;
            }
            var oldEvent = _eventRepository.LoadEvent(eventId);
            if (oldEvent == null)
            {
                Log.Information(
                    $"Can't edit event in tracker trackerId={trackerId} userId={trackerCreatorId} eventId={eventId}. Event does not existed");
                return false;
            } 
            if (eventId != newEvent.Id)
            {
                Log.Information(
                    $"Can't edit event in tracker trackerId={trackerId} userId={trackerCreatorId} eventId={eventId}. EventId does not match");
                return false;
            }
            _eventRepository.DeleteEvent(eventId);
            _eventRepository.AddEvent(newEvent);
            _eventTrackerRepository.SaveTracker(tracker);
            Log.Information(
                $"Event Added trackerId={trackerId} userId={trackerCreatorId} eventId={newEvent.Id}");
            return true;
        }

        public Option<IList<Event>> GetAllEventsFromTracker(Guid trackerId, Guid trackerCreatorId)
        {
            // var tracker = _eventTrackerRepository.LoadTracker(trackerId);
            // if (userId != tracker.creatorId)
            // {
            //     Log.Information(
            //         $"Can't get events from tracker trackerId={trackerId} userId={userId}. TrackerCreatorId does not match");
            //     return Option<IList<Event>>.None;
            // }
            //
            // Log.Information($"Returned events from trackerId={trackerId}");
            // return Option<IList<Event>>.Some(tracker);
            throw new NotImplementedException();
        }

        // public Option<IReadOnlyCollection<Event>> GetEventsFiltratedByTime(Guid userId,
        //     Guid trackerId,
        //     DateTimeOffset from, DateTimeOffset to)
        // {
        //     var tracker = _eventTrackerRepository.LoadTracker(trackerId);
        //     if (userId != tracker.creatorId)
        //     {
        //         Log.Information(
        //             $"Can't filter events from tracker trackerId={trackerId} userId={userId}. TrackerCreatorId does not match");
        //         return Option<IReadOnlyCollection<Event>>.None;
        //     }
        //     var requiredTracker = _eventTrackerRepository.LoadTracker(trackerId);
        //     Log.Information($"Get Filtered from {from} to {to} events from trackerId={trackerId}");
        //     return Option<IReadOnlyCollection<Event>>.Some(requiredTracker.FilterEventsByTimeSpan(from, to));
        // }
    }
}

