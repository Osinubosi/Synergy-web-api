using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Synergy.Domain.Constants;
using Synergy.Domain.Implementation;
using Synergy.Domain.Interfaces;
using Synergy.Domain.ServiceModel;
using Synergy.Repository.Database;
using Synergy.Repository.Interfaces;
using Synergy.Service.Implementations;
using Synergy.Service.Interfaces;
using Synergy_web_api.WebHandler;

namespace Synergy_web_api
{
    public class Startup
    {
        private const string SECRET_KEY = "iNivDmHLpUA223sqsfhqGbMRdRjEKOBL";

        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SECRET_KEY));
        private readonly IHostEnvironment _env;
        public Startup(IConfiguration configuration,IHostEnvironment evn)
        {
            Configuration = configuration;
            _env = evn;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            DatabaseContext(services);

            ApiBehaviour(services);

            ConfigureSwagger(services);
            AuthorizationService(services);


            services.AddSingleton(Configuration);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure<EmailConfigurationProvider>(Configuration.GetSection("EmailConfigurationProvider"));
            services.AddMemoryCache();
            services.AddScoped<IDbContext, SynergyDbContext>();
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IOnboardingService, OnboardingService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<ISynergySettings, SynergySettings>();
            services.AddTransient<ICryptographyService, CryptographyService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IHttpService, HttpService>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                        corsBbuilder =>
                        {
                            corsBbuilder.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials()
                                .WithExposedHeaders("Authorization", "WWW-Authenticate", "X-Pagination");
                        });
            });
        }

        private void DatabaseContext(IServiceCollection services)
        {
            if (_env.IsDevelopment())
                services.AddDbContext<SynergyDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("SynergyDbConnection")));
            if (_env.IsProduction())
                services.AddDbContext<SynergyDbContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("SynergyDbConnectionProduction")));

            services.AddSingleton<IJwtFactory, JwtFactory>();
        }

        private void AuthorizationService(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(JwtClaimIdentifiers.Role, JwtClaimIdentifiers.ApiAccess));
            });
            services.AddOptions();

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

           

            var tokenVaidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenVaidationParameters;
                configureOptions.SaveToken = true;
            });


        }

        private static void ApiBehaviour(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
          
        }

        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Bearer Token authentication"
                });
                c.OperationFilter<HeaderFilter>();

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Synergy API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
