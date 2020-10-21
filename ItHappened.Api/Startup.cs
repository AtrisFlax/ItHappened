using System.Text;
using ItHappened.Api.Authentication;
using ItHappened.Api.Options;
using ItHappened.Application.Services.EventTrackerService;
using ItHappened.Application.Services.UserService;
using ItHappened.Domain;
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
            services.AddControllers();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IEventTrackerService, EventTrackerService>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IEventTrackerRepository, EventTrackerRepository>();
            services.AddSingleton<IEventRepository, EventRepository>();
            
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

            
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo {Title = "ItHappened API", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);
            app.UseSwagger((options) => options.RouteTemplate = swaggerOptions.JsonRoute);
            app.UseSwaggerUI((options) => options.SwaggerEndpoint(swaggerOptions.UiEndpoint,
                swaggerOptions.ApiDescription));
            
            var jwtOptions = new JwtOptions();
            Configuration.GetSection(nameof(JwtOptions)).Bind(jwtOptions);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}