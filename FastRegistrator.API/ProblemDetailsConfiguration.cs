using FastRegistrator.Application.Exceptions;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;

namespace FastRegistrator.API
{
    public static class ProblemDetailsConfiguration
    {
        public static void AddConfiguredProblemDetails(this IServiceCollection services)
        {            
            services.AddProblemDetails(options =>
            {
                options.IncludeExceptionDetails = (ctx, ex) => false;

                options.MapFluentValidationException();
                options.MapToStatusCode<NotFoundException>(StatusCodes.Status404NotFound);
                options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
            });
        }

        private static void MapFluentValidationException(this ProblemDetailsOptions options) =>
            options.Map<ValidationException>((ctx, ex) =>
            {
                var factory = ctx.RequestServices.GetRequiredService<ProblemDetailsFactory>();

                var errors = ex.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Select(x => x.ErrorMessage).ToArray());

                return factory.CreateValidationProblemDetails(ctx, errors, StatusCodes.Status400BadRequest);
            });
    }
}
