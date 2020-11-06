using AutoMapper;
using ItHappened.Domain;
using ItHappened.Infrastructure.Dto;

namespace ItHappened.Infrastructure.Mappers
{
    public class DbToDomainMappingProfiles : Profile
    {
        public DbToDomainMappingProfiles()
        {
            CreateMap<UserDto, User>();
            CreateMap<EventTrackerDto, EventTracker>().ConvertUsing<EventTrackerDtoToEventTrackerConverter>();
            CreateMap<EventDto, Event>().ConvertUsing<EventDtoToEventConverter>();
        }
    }
}