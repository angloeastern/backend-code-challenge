using System.Net;
using System.Threading.RateLimiting;
using AEBackend.Repositories;
using AEBackend.Repositories.RepositoryUsingEF;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AEBackend;

public class Program
{
    public Program()
    {

    }

    public async Task SeedData(WebApplication app)
    {
        var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

        using (var scope = scopedFactory?.CreateScope())
        {
            var service = scope?.ServiceProvider.GetService<Seeder>();
            await service!.Seed();
        }
    }

    public async Task ApplyMigrations(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<UserDBContext>();

        Console.WriteLine("Connection string:" + context.Database.GetConnectionString());

        if ((await context.Database.GetPendingMigrationsAsync()).Any())
        {
            await context.Database.MigrateAsync();
        }
    }

    private void SetupApiRateLimiter(WebApplicationBuilder builder)
    {
        builder.Services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter(policyName: "fixed", options =>
                {
                    options.PermitLimit = 2;
                    options.Window = TimeSpan.FromSeconds(30);
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = 2;

                });
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.OnRejected = async (context, token) =>
            {
                await context.HttpContext.Response.WriteAsync("Too many requests. Please try again latter");
            };
        });
    }

    private void SetupSwaggerAndApiVersioning(WebApplicationBuilder builder)
    {
        builder.Services.AddApiVersioning(
            options =>
            {
                options.ReportApiVersions = true;
            })
        .AddApiExplorer(
            options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        // builder.Services.AddEndpointsApiExplorer().AddApiVersioning();
        builder.Services.AddSwaggerGen();

    }

    private void SetupSwagger(WebApplication app)
    {

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                    options =>
                    {
                        var descriptions = app.DescribeApiVersions();
                        foreach (var description in descriptions)
                        {
                            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                        }
                    });


        }
    }

    private void SetupDBContexts(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<UserDBContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

    }

    private void SetupCORS(WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowAll", builder =>
                    {
                        builder.AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin();
                    });
                });
    }

    private void SetupCustomServices(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<Seeder>();
        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        builder.Services.AddTransient<IUserRepository, UserRepositoryUsingEF>();
    }

    public WebApplication CreateApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Logging.AddConsole();

        SetupApiRateLimiter(builder);
        SetupSwaggerAndApiVersioning(builder);
        SetupDBContexts(builder);
        SetupCORS(builder);
        SetupCustomServices(builder);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            SetupSwagger(app);
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
    public async Task Run(string[] args)
    {

        var app = CreateApp(args);

        await ApplyMigrations(app);
        await SeedData(app);
        app.Run();
    }

    public static async Task Main(string[] args)
    {
        Console.WriteLine("Calling Program().Run()...");
        await new Program().Run(args);
    }
}


