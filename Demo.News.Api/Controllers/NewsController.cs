using AutoMapper;
using Demo.News.Api.Dtos;
using Demo.News.Api.Helpers;
using Demo.News.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Demo.News.Api.Controllers
{
    [ApiVersion("1")]
    [Route("api/news/v{version:apiVersion}")]
    [ApiController]
    [ApiConventionType(typeof(NewConvensions))]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _news;
        private readonly IMapper _mapper;

        public NewsController(INewsService news,IMapper mapper)
        {
            _news = news;
            _mapper = mapper;
        }

        /// <summary>
        /// Endpoint to get the top news stories
        /// </summary>
        /// <remarks>
        /// Endpoint Response details
        /// <ul>
        /// <li>Endpoint returns the list of stories with title and url for details  </li>
        /// </ul>
        /// </remarks>
        [HttpGet("stories")]
        public async Task<ActionResult<List<StoryReadDto>>> GetTopStories()
        {
            var stories = await _news.GetStories();
            List<StoryReadDto> result=_mapper.Map<List<StoryReadDto>>(stories);
            return Ok(result);
        }
    }
}
