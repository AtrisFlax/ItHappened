using System.Collections.Generic;
using AutoMapper;
using ItHappened.Api.Contracts.Responses;
using ItHappened.Domain;

namespace ItHappened.Api.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<EventTracker, TrackerResponse>();
            CreateMap<Event, EventResponse>();
            CreateMap<TrackerCustomizationSettings, CustomizationSettingsResponse>();


            //Mapping example
            /*CreateMap<Post, PostResponse>()
                .ForMember(dest => dest.Tags, opt => 
                    opt.MapFrom(src => src.Tags.Select(x => new TagResponse{Name = x.TagName})));
                    */

        }
    }
}