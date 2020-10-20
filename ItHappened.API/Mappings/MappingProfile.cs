using AutoMapper;
using ItHappened.Domain.Statistics;

namespace ItHappened.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //написал пока object, так как не создан ни один класс для маппинга
            CreateMap<IStatisticsFact, object>();
        }
    }
}