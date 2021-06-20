using System.Linq;
using API.Errors;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        /// <summary>
        /// We are injecting ServiceCollection and configuring its values.
        /// In this way we keep the Startup.cs clean.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //The AddScoped is related with the lifetime of the service. Scoped litefime services are created once per request.
            services.AddScoped<IProductRepository, ProductRepository>();
            //The way we add the Generic is different because we don't know the type
            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));

            //in this particular case that follows, the order is important.
            //This configuration makes the customization of error message of a bad call of the API.
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                    .Where(error => error.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            return services;
        }
    }
}