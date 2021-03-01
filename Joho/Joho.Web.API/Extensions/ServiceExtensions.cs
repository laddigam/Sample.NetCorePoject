using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Joho.Services.Config;
using Joho.Services.Entities;
using Joho.Services.Repository.Abstract;
using Joho.Services.Repository.Concrete;
using Joho.Services.Abstract;
using Joho.Services.Concrete;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Joho.Web.API.Filters;
using System.Text;


namespace Joho.Web.API.Extensions
{
    /// <summary>
    /// Class for providing extended service functionality
    /// </summary>
    public static class ServiceExtensions
    {

        /// <summary>
        /// CORS- Cross-Origin Resource Sharing policy
        /// </summary>
        /// <param name="services">The services.</param>
        public static void AddCors(this IServiceCollection services)
        {
 
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin()
         .AllowAnyHeader()
         .AllowAnyMethod());
        
         //.SetIsOriginAllowed((host) => true));
            });
        }

        /// <summary>
        /// IIS configuration
        /// </summary>
        /// <param name="services">The services.</param>
        public static void AddIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {
            });
        }

        /// <summary>
        /// Adds my SQL context.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="config">The configuration.</param>
        public static void AddMySqlContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["ConnectionStrings:DatabaseConnection"];
            
            //services.AddDbContext<RepositoryContext>(o => o.UseMySql(connectionString));
            services.AddTransient<DataBaseConnector>(_ => new DataBaseConnector(connectionString));
            
        }

        /// <summary>
        /// Adds the repository wrapper.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        public static void AddRepositoryWrapper(this IServiceCollection services, IConfiguration configuration)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfileConfiguration());
            });
            services.AddSingleton<IMapper>(sp => mappingConfig.CreateMapper());
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddScoped<IUserDetailsManager, UserDetailsManager>((ctx) =>
            {
                IRepositoryWrapper repository = ctx.GetService<IRepositoryWrapper>();
               IMapper mapper = ctx.GetService<IMapper>();
               return new UserDetailsManager(repository, mapper);
            });

            services.AddScoped <ISalaryDetailsManager, SalaryDetailsManager>((ctx) =>
            {
                IRepositoryWrapper repository = ctx.GetService<IRepositoryWrapper>();
                IMapper mapper = ctx.GetService<IMapper>();
                return new SalaryDetailsManager( repository, mapper);
            });

            

     

        }

        /// <summary>
        /// Adds the automapper.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void AddAutomapper(this IServiceCollection services)
        {
            //Register Automapper
            services.AddAutoMapper(typeof(MappingProfileConfiguration));
        }

        /// <summary>
        /// Adds the JWT token service.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="Configuration">The configuration.</param>
        public static void AddJwtTokenService(this IServiceCollection services, IConfiguration Configuration)
        {
            
            // ===== Add Jwt Authentication ========
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        //ValidIssuer = Configuration["JwtIssuer"],
                        //ValidAudience = Configuration["JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String("856FECBA3B06519C8DDDBC80BB080553")),
                        // Ensure the token hasn't expired:                       
                        ValidateLifetime = false,
                        ClockSkew = TimeSpan.Zero
                    };

                });
        }

        ///// <summary>
        ///// Adds the email service.
        ///// </summary>
        ///// <param name="services">The services.</param>
        ///// <param name="configuration">The configuration.</param>
        //public static void AddEmailService(this IServiceCollection services, IConfiguration configuration)
        //{
        //    var smtpConfiguration = configuration.GetSection(nameof(SmtpConfiguration)).Get<SmtpConfiguration>();
        //    if (smtpConfiguration != null && !string.IsNullOrWhiteSpace(smtpConfiguration.Host))
        //    {
        //        services.AddSingleton(smtpConfiguration);
        //        services.AddTransient<IEmailSender, EmailSender>();
        //    }

        //}
        /// <summary>
        /// Adds the authorization policy.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void AddAuthorizationPolicy(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.AdminOnly, policy =>
                           policy.RequireClaim("UserTypeId", "1", "2"));
            });
        }

        /// <summary>
        /// Adds the custom bad requset.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void AddCustomBadRequset(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(a =>
            {
                a.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new CustomBadRequest(context);
                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json", "application/problem+xml" }
                    };
                };
            });
        }
    }
}
