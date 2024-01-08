using Demo.News.Api.Configuration;
using Demo.News.Api.Entities;
using Demo.News.Api.Wrapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Demo.News.Api.Services
{
    public class NewsService: INewsService
    {
        private readonly Settings _settings;
        private readonly IMemoryCache _cache;
        private readonly IHttpClientWrapper _httpClientWrapper;
        private const string cacheKey = "CacheKey";

        public NewsService(IOptions<Settings> settings,IMemoryCache cache,IHttpClientWrapper httpClientWrapper)
        {
            _settings = settings.Value;
            _cache = cache;
            _httpClientWrapper = httpClientWrapper;
        }

        private async Task<List<int>> GetTopStories()
        {
            HttpResponseMessage response = await _httpClientWrapper.GetAsync($"{_settings.HackerNewBaseUrl}/topstories.json");
            string serialized = await response.Content.ReadAsStringAsync();
            var resp = JsonSerializer.Deserialize<List<int>>(serialized);
            return resp;
        }

        private async Task<Story> GetStory(string url)
        {
            HttpResponseMessage response = await _httpClientWrapper.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string serialized = await response.Content.ReadAsStringAsync();
                var resp = JsonSerializer.Deserialize<Story>(serialized);
                return resp;
            }
            else return null;
        }


        public async Task<List<Story>> GetStories()
        {
            
            try
            {
                if (_cache.TryGetValue(cacheKey, out List<Story> stories))
                {
                    return stories;
                }
                else
                {
                    //TODO - Can be lock and unlock with semaphore later
                    var topStories = await GetTopStories();
                    topStories = topStories.Take(200).ToList();
                    var tasks = new List<Task<Story>>();
                    foreach (var id in topStories)
                    {
                        string url = $"{_settings.HackerNewBaseUrl}/item/{id}.json";
                        tasks.Add(GetStory(url));
                    }
                    await Task.WhenAll(tasks);
                    stories = new List<Story>();
                    foreach (var task in tasks)
                    {
                        if(task.Result!=null)
                         stories.Add(task.Result);
                    }
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                                  .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                                  .SetPriority(CacheItemPriority.Normal);
                    _cache.Set(cacheKey, stories, cacheEntryOptions);
                    return stories;
                }
            }
            catch (Exception)
            {
                throw;
            }
            
        }

    }
}
