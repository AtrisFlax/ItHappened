using AutoMapper;
using ItHappened.Domain;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    public class DbToDomainProfile : Profile
    {
        public DbToDomainProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}