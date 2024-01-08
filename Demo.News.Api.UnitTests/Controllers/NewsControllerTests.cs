using AutoMapper;
using Demo.News.Api.Controllers;
using Demo.News.Api.Dtos;
using Demo.News.Api.Entities;
using Demo.News.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Demo.News.Api.UnitTests.Controllers
{
    public  class NewsControllerTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<INewsService> _newsServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly NewsController _sut;

        public NewsControllerTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());
            _newsServiceMock = _fixture.Freeze<Mock<INewsService>>();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _sut = new NewsController(_newsServiceMock.Object,_mapperMock.Object);
        }

        [Fact]
        public async Task GetTopStories_ValidRequest_ReturnsStories()
        {
            //Arrage
            var resultFormService = _fixture.Create<List<Story>>();
            var expected=_fixture.Create<List<StoryReadDto>>();
            _newsServiceMock.Setup(news => news.GetStories()).ReturnsAsync(resultFormService);

            //Act
            var actual=await _sut.GetTopStories();

            //Assert
            var result = actual.Result.Should().BeOfType<OkObjectResult>().Subject;
            var stories = result.Value.Should().BeAssignableTo<List<StoryReadDto>>().Subject;
        }
    }
}
