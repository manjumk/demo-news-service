using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Demo.News.Api.Extentions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            services.AddOptions<SwaggerGenOptions>()
                .Configure<IApiVersionDescriptionProvider>((setupAction, provider) =>
                {
                    foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
                    {
                        setupAction.SwaggerDoc(description.GroupName, new OpenApiInfo()
                        {
                            Title = "News Service",
                            Version = description.ApiVersion.ToString(),
                            Description = "News stories related - REST API operations"
                        }); ;
                    }
                });

            services.AddSwaggerGen(setupAction =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                setupAction.IncludeXmlComments(xmlPath);
            });
            return services;
        }

        public static IApplicationBuilder AddSwaggerMiddleware(this WebApplication app)
        {
            var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                foreach (ApiVersionDescription description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"../swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
                c.DefaultModelExpandDepth(2);
                c.DisplayRequestDuration();
                c.EnableDeepLinking();
            });

            return app;
        }
    }
}
