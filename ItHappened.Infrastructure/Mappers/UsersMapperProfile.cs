using AutoMapper;
using ItHappened.Domain;

namespace ItHappened.Infrastructure.Mappers
{
    public class UsersMapperProfile : Profile
    {
        public UsersMapperProfile()
        {
            CreateMap<User, UserDbEntity>()
                .ForMember(dest => dest.Hash,
                    opt => opt.MapFrom(src => src.Password.Hash))
                .ForMember(dest => dest.Salt,
                    opt => opt.MapFrom(src => src.Password.Salt));
        }
    }
}