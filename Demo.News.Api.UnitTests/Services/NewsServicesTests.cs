using Demo.News.Api.Configuration;
using Demo.News.Api.Entities;
using Demo.News.Api.Services;
using Demo.News.Api.Wrapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Demo.News.Api.UnitTests.Services
{
    public class NewsServicesTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IOptions<Settings>> _settingsMock;
        private readonly Mock<IMemoryCache> _cacheMock;
        private readonly Mock<IHttpClientWrapper> _wrapperMock;
        private readonly NewsService _sut;

        public NewsServicesTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());
            _settingsMock = _fixture.Freeze<Mock<IOptions<Settings>>>();
            _cacheMock = _fixture.Freeze<Mock<IMemoryCache>>();
            _wrapperMock = _fixture.Freeze<Mock<IHttpClientWrapper>>();
            _sut = new NewsService(_settingsMock.Object, _cacheMock.Object, _wrapperMock.Object);
        }

        [Fact]
        public void GetStories_ValidInput_ReturnStories()
        {
            //Arrage
            var storyIds = _fixture.CreateMany<int>(2);
            var firstStory = _fixture.Create<Story>();
            var secondStory = _fixture.Create<Story>();
            var expected=new List<Story>() { firstStory, secondStory };

            _wrapperMock.SetupSequence(httprequest => httprequest.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(storyIds))
            })
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode=System.Net.HttpStatusCode.OK,
                Content=new StringContent(JsonSerializer.Serialize(firstStory))
            })
            .ReturnsAsync(new HttpResponseMessage()
             {
                 StatusCode = System.Net.HttpStatusCode.OK,
                 Content = new StringContent(JsonSerializer.Serialize(secondStory))
             });

            //Act
            var actual = _sut.GetStories();

            //Assert   
            var stories= actual.Result.Should().BeAssignableTo<List<Story>>().Subject;
            stories.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void GetStories_ValidInput_ReturnSingleStoryWithOneFailed()
        {
            //Arrage
            var storyIds = _fixture.CreateMany<int>(2);
            var firstStory = _fixture.Create<Story>();
            var secondStory = _fixture.Create<Story>();
            var expected = new List<Story>() { firstStory };

            _wrapperMock.SetupSequence(httprequest => httprequest.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(storyIds))
            })
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(firstStory))
            })
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest
            });

            //Act
            var actual = _sut.GetStories();

            //Assert   
            var stories = actual.Result.Should().BeAssignableTo<List<Story>>().Subject;
            stories.Should().BeEquivalentTo(expected);

        }

        [Fact]
        public async Task GetStories_ValidInput_ThrowsException()
        {
            // Arrange
            _wrapperMock.Setup(w => w.GetAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Service error"));

            // Act and Assert
            var actual = await Assert.ThrowsAsync<Exception>(async () => await _sut.GetStories());
            actual.Message.Should().Be("Service error");
        }


        [Fact]
        public async Task GetStories_ValidInputWithCache_ReturnsStories()
        {
            // Arrange
            var token = "CacheKey";
            var expected=_fixture.CreateMany<Story>();

            // The below line should actually mock the cache hit scenario, but for some reason it isn't working yet.
            //_cacheMock.Setup(c => c.TryGetValue(token,out expected)).Returns(true);
            //_cacheMock.Setup(c => c.CreateEntry(token)).Returns(Mock.Of<ICacheEntry>);

            // Act
            var actual = _sut.GetStories();

            // Assert
            var result=actual.Result.Should().BeAssignableTo<List<Story>>();
            result.Should().BeEquivalentTo(expected);
        }
    }
}
