using AutoMapper;
using ItHappened.Domain;

namespace ItHappened.Infrastructure.EFCoreRepositories.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Event, EventDto>();
        }
    }
}