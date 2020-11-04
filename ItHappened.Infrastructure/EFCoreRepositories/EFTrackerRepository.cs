using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ItHappened.Application.Errors;
using ItHappened.Domain;
using ItHappened.Infrastructure.Mappers;
using LanguageExt.UnsafeValueAccess;
using Microsoft.EntityFrameworkCore;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    // ReSharper disable once InconsistentNaming
    public class EFTrackerRepository : ITrackerRepository
    {
        private readonly ItHappenedDbContext _context;
        private readonly IMapper _mapper;

        public EFTrackerRepository(ItHappenedDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void SaveTracker(EventTracker newTracker)
        {
            var trackerDto = _mapper.Map<EventTrackerDto>(newTracker);
            _context.EventTrackers.Add(trackerDto);
        }

        public EventTracker LoadTracker(Guid eventTrackerId)
        {
            var trackerDto = _context.EventTrackers.Find(eventTrackerId);
            return _mapper.Map<EventTracker>(trackerDto);
        }

        public IReadOnlyCollection<EventTracker> LoadAllUserTrackers(Guid userId)
        {
            var trackersDto = _context.EventTrackers.Where(tracker => tracker.CreatorId == userId);
            return _mapper.Map<EventTracker[]>(trackersDto.ToList());
        }

        public void UpdateTracker(EventTracker eventTracker)
        {
            var trackerDto = _mapper.Map<EventTrackerDto>(eventTracker);
            var local = _context.Set<EventTrackerDto>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(eventTracker.Id));
            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }

            _context.Entry(trackerDto).State = EntityState.Modified;
        }

        public void DeleteTracker(Guid eventTrackerId)
        {
            var trackerToDeleteDto = _context.EventTrackers.Find(eventTrackerId);
            _context.EventTrackers.Remove(trackerToDeleteDto);
        }

        public bool IsContainTracker(Guid trackerId)
        {
            return _context.EventTrackers.Any(tracker => tracker.Id == trackerId);
        }

        public bool IsExistTrackerWithSameName(Guid creatorId, string trackerName)
        {
            return _context.EventTrackers.Any(tracker => tracker.CreatorId == creatorId && tracker.Name == trackerName);
        }
    }
}