using AutoMapper;
using ItHappened.Domain;

namespace ItHappened.Infrastructure.Mappers
{
    public class DbToDomainMappingProfiles : Profile
    {
        public DbToDomainMappingProfiles()
        {
            CreateMap<UserDto, User>();
        }
    }
}
