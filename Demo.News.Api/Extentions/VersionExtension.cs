using Microsoft.AspNetCore.Mvc;

namespace Demo.News.Api.Extentions
{
    public static class VersionExtension
    {
        public static IServiceCollection AddVersionService(this IServiceCollection service)
        {
            service.AddVersionedApiExplorer(setupAction =>
            {
                setupAction.GroupNameFormat = "'v'V";
                setupAction.SubstituteApiVersionInUrl = true;
            });

            service.AddApiVersioning(setupAction => 
            {
                setupAction.AssumeDefaultVersionWhenUnspecified = true;
                setupAction.DefaultApiVersion = new ApiVersion(1, 0);
                setupAction.ReportApiVersions = true;
            });
            return service;
        }
    }
}
