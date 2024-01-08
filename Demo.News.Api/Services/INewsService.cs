using Demo.News.Api.Entities;

namespace Demo.News.Api.Services
{
    public interface INewsService
    {
        public Task<List<Story>> GetStories();
    }
}
