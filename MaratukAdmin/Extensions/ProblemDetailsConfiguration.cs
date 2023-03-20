using MaratukAdmin.Exceptions;
using Hellang.Middleware.ProblemDetails;

namespace MaratukAdmin.Extensions
{
    public static class ProblemDetailsConfiguration
    {
        public static IServiceCollection AddProblemDetails(this IServiceCollection services,
                                                            IConfiguration configuration,
                                                            IWebHostEnvironment environment)
        {
            return services.AddProblemDetails(options =>
            {
                options.IncludeExceptionDetails = (ctx, env) => true;

                options.Map<ApiBaseException>(exception =>
                {
                    return exception.ToProblemDetails();
                });

                options.OnBeforeWriteDetails = (ctx, pr) =>
                {
                    //here you can do the logging
                    pr.Extensions["exceptionDetails"] = null;
                };
            });
        }
    }
}
