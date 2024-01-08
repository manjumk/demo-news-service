using AutoMapper;
using Demo.News.Api.Entities;

namespace Demo.News.Api.Dtos
{
    //TODO- All profile class can be moved under profile folder.
    public class StoryProfile:Profile
    {
        public StoryProfile()
        {
            CreateMap<Story, StoryReadDto>();
        }
    }
}
