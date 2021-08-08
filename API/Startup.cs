using API.Extensions;
using API.Helpers;
using API.Middleware;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddControllers();
            services.AddDbContext<StoreContext>(context => context.UseSqlite(_config.GetConnectionString("DefaultConnection")));

            // Redis is used in our basket
            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var configuration = ConfigurationOptions.Parse(_config.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configuration);
            });

            // Using the services applications present in ApplicationServicesExtensions.cs
            services.AddApplicationServices();
            // Using the services applications present in SwaggerServiceExtensions.cs
            services.AddSwaggerDocumentation();
            // To use CORS
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
               {
                   policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
               });
            });

        }

        /// <summary>
        /// This method gets called by runtime. Use this method to configure the HTTP request pipeline.
        /// The order is important.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Uses the ExceptionMiddleware to call a custom error when an Exception occurs.
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseSwaggerDocumentation();
            //The method below allows the application to reach an error page even when the endpoint doesn't exist.
            //The placeholder {0} represents the StatusCode
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles();

            //To use CORS
            app.UseCors("CorsPolicy");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
