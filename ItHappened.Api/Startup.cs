using System.Text;
using AutoMapper;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.MemoryStorage;
using ItHappened.Api.Authentication;
using ItHappened.Api.MappingProfiles;
using ItHappened.Api.Middleware;
using ItHappened.Api.Models.Requests;
using ItHappened.Api.Options;
using ItHappened.Application.Authentication;
using ItHappened.Application.Services.EventService;
using ItHappened.Application.Services.StatisticService;
using ItHappened.Application.Services.TrackerService;
using ItHappened.Application.Services.UserService;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure;
using ItHappened.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace ItHappened.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddFluentValidation(cfg =>
                {
                    cfg.RegisterValidatorsFromAssemblyContaining<TrackerRequest>();
                });
            //repos
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<ITrackerRepository, TrackerRepository>();
            services.AddSingleton<IEventRepository, EventRepository>();
            services.AddSingleton<ISingleFactsRepository, SingleFactsRepository>();
            services.AddSingleton<IMultipleFactsRepository, MultipleFactsRepository>();
            
            //app services
            
            services.AddSingleton<IEventService, EventService>();
            services.AddSingleton<ITrackerService, TrackerService>();
            
            AddMultipleTrackersStatisticsProvider(services);
            AddSingleTrackerStatisticsProvider(services);

            services.AddSingleton<IBackgroundStatisticGenerator, StatisticGenerator>();
            services.AddSingleton<IManualStatisticGenerator, StatisticGenerator>();
            
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IStatisticsService, StatisticsService>();
            
            //mappers
            services.AddAutoMapper(typeof(Startup));
            services.AddSingleton<IMyMapper, MyMapper>();
            
            //jwt
            var jwtOptions = new JwtOptions();
            Configuration.GetSection(nameof(JwtOptions)).Bind(jwtOptions);
            services.AddSingleton(jwtOptions);
            services.AddSingleton<IJwtIssuer, JwtIssuer>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.Secret)),
                        ValidateLifetime = true
                    };
                });

            //swagger
            services.AddSwaggerGen(swaggerGenOptions =>
            {
                swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo {Title = "ItHappened API", Version = "v1"});
                // Bearer token authentication
                var securityDefinition = new OpenApiSecurityScheme()
                {
                    Name = "Bearer",
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Description = "Specify the authorization token.",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                };
                swaggerGenOptions.AddSecurityDefinition("jwt_auth", securityDefinition);

                // Make sure swagger UI requires a Bearer token specified
                var securityScheme = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference()
                    {
                        Id = "jwt_auth",
                        Type = ReferenceType.SecurityScheme
                    }
                };
                var securityRequirements = new OpenApiSecurityRequirement()
                {
                    {securityScheme, new string[] { }},
                };
                swaggerGenOptions.AddSecurityRequirement(securityRequirements);
            });

            services.AddHangfire(configuration => configuration.UseMemoryStorage());
            services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }

            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);
            app.UseSwagger(options => options.RouteTemplate = swaggerOptions.JsonRoute);
            app.UseSwaggerUI(options => options.SwaggerEndpoint(swaggerOptions.UiEndpoint,
                swaggerOptions.ApiDescription));
            
            var jwtOptions = new JwtOptions();
            Configuration.GetSection(nameof(JwtOptions)).Bind(jwtOptions);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireDashboard();
            
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
        }

        private void AddMultipleTrackersStatisticsProvider(IServiceCollection services)
        {
            var statisticsProvider = new MultipleTrackersFactProvider();
            statisticsProvider.Add(new MostEventfulWeekCalculator());
            statisticsProvider.Add(new MostEventfulDayStatisticsCalculator());
            statisticsProvider.Add(new MostFrequentEventStatisticsCalculator());
            statisticsProvider.Add(new MultipleTrackersEventsCountCalculator());
            services.AddSingleton<IMultipleTrackersFactProvider>(statisticsProvider);
        }
        
        private void AddSingleTrackerStatisticsProvider(IServiceCollection services)
        {
            var statisticsProvider = new SingleTrackerFactProvider();
            statisticsProvider.Add(new AverageRatingCalculator());
            statisticsProvider.Add(new AverageScaleCalculator());
            statisticsProvider.Add(new BestRatingEventCalculator());
            statisticsProvider.Add(new LongestBreakCalculator());
            statisticsProvider.Add(new OccursOnCertainDaysOfTheWeekCalculator());
            statisticsProvider.Add(new SingleTrackerEventsCountCalculator());
            statisticsProvider.Add(new SumScaleCalculator());
            statisticsProvider.Add(new WorstEventCalculator());
            services.AddSingleton<ISingleTrackerFactProvider>(statisticsProvider);
        }
    }
}