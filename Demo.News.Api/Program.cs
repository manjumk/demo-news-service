using Demo.News.Api.Configuration;
using Demo.News.Api.Extentions;
using Demo.News.Api.Middlewares;
using Demo.News.Api.Services;
using Demo.News.Api.Wrapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddVersionService();
builder.Services.AddSwaggerService();
builder.Services.AddAutoMapper(typeof(Program));

//Can be added as grouped in extension
builder.Services.AddScoped<INewsService,NewsService>();
builder.Services.AddSingleton<IHttpClientWrapper, HttpClientWrapper>();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

builder.Services.Configure<Settings>(options => builder.Configuration.GetSection("Settings").Bind(options));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.AddSwaggerMiddleware();
}

app.UseAuthorization();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.MapControllers();

app.Run();
