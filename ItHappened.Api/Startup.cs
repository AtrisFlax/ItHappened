using System.Text;
using AutoMapper;
using Hangfire;
using Hangfire.MemoryStorage;
using ItHappened.Api.Authentication;
using ItHappened.Api.Mapping;
using ItHappened.Api.Mapping.ItHappened.Api.MappingProfiles;
using ItHappened.Api.Middleware;
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

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //auto mapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new RequestToDomainProfile());
                cfg.AddProfile(new DomainToResponseProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            //FactsToJsonMapper
            services.AddSingleton<IFactsToJsonMapper, FactsToNewtonJsonMapper>();

            //service repos
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

            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IStatisticsService, StatisticsService>();


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

            //hangfire
            services.AddHangfire(configuration => configuration.UseMemoryStorage());
            services.AddHangfireServer();
            
            //skip null in json 
            services.AddMvc().AddJsonOptions(options => { options.JsonSerializerOptions.IgnoreNullValues = true; });

            services.AddControllers();
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
            statisticsProvider.Add(new MostEventfulDayCalculator());
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
            statisticsProvider.Add(new WorstRatingEventCalculator());
            services.AddSingleton<ISingleTrackerFactProvider>(statisticsProvider);
        }
    }
}