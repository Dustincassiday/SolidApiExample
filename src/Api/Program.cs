using MediatR;
using SolidApiExample.Api.Errors;
using SolidApiExample.Application.Orders.CreateOrder;
using SolidApiExample.Application.Orders.GetOrder;
using SolidApiExample.Application.Orders.ListOrders;
using SolidApiExample.Application.Orders.Shared;
using SolidApiExample.Application.Orders.UpdateOrder;
using SolidApiExample.Application.People.CreatePerson;
using SolidApiExample.Application.People.DeletePerson;
using SolidApiExample.Application.People.GetPerson;
using SolidApiExample.Application.People.ListPeople;
using SolidApiExample.Application.People.Shared;
using SolidApiExample.Application.People.UpdatePerson;
using SolidApiExample.Application.Pipeline;
using SolidApiExample.Application.Repositories;
using SolidApiExample.Application.Validation;
using SolidApiExample.Infrastructure.Repositories.InMemory;
using SolidApiExample.Api.Auth;
using Microsoft.OpenApi.Models;

namespace SolidApiExample.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var app = BuildApp(args);
        await app.RunAsync();
    }

    internal static WebApplication BuildApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();
        ConfigurePipeline(app);
        return app;
    }

    internal static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(ApiKeyDefaults.Scheme, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Name = ApiKeyDefaults.HeaderName,
                Description = "API key required to authorize requests."
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = ApiKeyDefaults.Scheme
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
        services.AddProblemDetails();
        services.AddExceptionHandler<ProblemDetailsExceptionHandler>();

        services
            .AddAuthentication(ApiKeyDefaults.Scheme)
            .AddScheme<ApiKeyOptions, ApiKeyAuthenticationHandler>(
                ApiKeyDefaults.Scheme,
                options =>
                {
                    options.ExpectedKey = configuration.GetValue<string>("Auth:ApiKey") ?? "dev-api-key";
                });

        services.AddAuthorization();

        services.AddSingleton<IPeopleRepo, InMemoryPeopleRepo>();
        services.AddSingleton<IOrdersRepo, InMemoryOrdersRepo>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreatePersonHandler>());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddScoped<IRequestValidator<GetPersonQuery>, GetPersonValidator>();
        services.AddScoped<IRequestValidator<ListPeopleQuery>, ListPeopleValidator>();
        services.AddScoped<IRequestValidator<CreatePersonCommand>, CreatePersonValidator>();
        services.AddScoped<IRequestValidator<UpdatePersonCommand>, UpdatePersonValidator>();
        services.AddScoped<IRequestValidator<DeletePersonCommand>, DeletePersonValidator>();

        services.AddScoped<IRequestValidator<GetOrderQuery>, GetOrderValidator>();
        services.AddScoped<IRequestValidator<ListOrdersQuery>, ListOrdersValidator>();
        services.AddScoped<IRequestValidator<CreateOrderCommand>, CreateOrderValidator>();
        services.AddScoped<IRequestValidator<UpdateOrderCommand>, UpdateOrderValidator>();
    }

    internal static void ConfigurePipeline(WebApplication app)
    {
        app.UseAuthentication();
        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapControllers();
    }
}
