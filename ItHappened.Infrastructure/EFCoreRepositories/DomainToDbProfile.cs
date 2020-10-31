using AutoMapper;
using ItHappened.Domain;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    public class DomainToDbProfile : Profile
    {
        public DomainToDbProfile()
        {
            CreateMap<UserDto, User>();
        }
    }
}