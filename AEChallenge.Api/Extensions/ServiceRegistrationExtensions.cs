using AEChallenge.Api.Middlewares;
using AEChallenge.Api.Services;
using AEChallenge.Application.Interfaces;
using AEChallenge.Infrastructure.Repositories;
using AEChallenge.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;

namespace AEChallenge.Api.Extensions
{
    public static class ServiceRegistrationExtensions
    {
        public static IApplicationBuilder UseApiKeyValidation(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyMiddleware>();
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ICrewRepository, CrewRepository>();
            services.AddScoped<IFinanceRepository, FinanceRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IShipRepository, ShipRepository>();

            services.AddSingleton<IAuthorizationMiddlewareResultHandler, ForbiddenHandler>();
            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]
                    ?? throw new ArgumentNullException("SecretKey not found in JwtSettings."));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;


                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();

                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";


                        var errorResponse = new
                        {
                            status = StatusCodes.Status401Unauthorized,
                            message = "Authentication Failed. The JWT token is missing, invalid, or expired."
                        };

                        var jsonResponse = JsonSerializer.Serialize(errorResponse);
                        return context.Response.WriteAsync(jsonResponse);
                    }
                };

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            return services;
        }
        public static IServiceCollection AddSwaggerSetup(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(type => type.FullName);


                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "AE Challenge Web API",
                    Version = "v1",
                    Description = "API documentation for the AE Challenge project."
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Description = "API Key required for service verification (AE_Key)",
                    Name = "AE_Key",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    new List<string>()
                },
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" }
                    },
                    new List<string>()
                }
            });
            });

            return services;
        }
        public static IServiceCollection AddCorsSetup(this IServiceCollection services, IConfiguration configuration)
        {
            var allowedOrigins = configuration.GetSection("CORS:AllowedOrigins").Get<string[]>() ?? [];
            var policyName = configuration["CORS:PolicyName"] ?? "DefaultCorsPolicy";

            services.AddCors(options =>
            {
                options.AddPolicy(name: policyName,
                    builder =>
                    {
                        builder.WithOrigins(allowedOrigins)
                               .AllowAnyHeader()
                               .AllowAnyMethod();

                    });
            });

            return services;
        }

    }

}
