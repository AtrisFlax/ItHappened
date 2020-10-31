using AutoMapper;
using ItHappened.Domain;

namespace ItHappened.Infrastructure.Mappers
{
    public class DomainToDbMappingProfiles : Profile
    {
        public DomainToDbMappingProfiles()
        {
            CreateMap<User, UserDto>();
        }
    }
}