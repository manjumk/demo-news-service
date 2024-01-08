using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Demo.News.Api.Helpers
{
    public static class NewConvensions
    {
        [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        //[Produces("application/json")]
        public static void Get()
        { }

        /*TODO :- Can be added more convensions for common method names */

        //[ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        //public static void Get(
        //[ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)]
        //[ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] int ids)
        //{ }

       
    }
}
