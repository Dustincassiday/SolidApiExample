using FluentValidation;
using MediatR;
using System.Reflection;
using SolidApiExample.Api.Errors;
using SolidApiExample.Application.Orders.CreateOrder;
using SolidApiExample.Application.Orders.GetOrder;
using SolidApiExample.Application.Orders.ListOrders;
using SolidApiExample.Application.Orders.UpdateOrder;
using SolidApiExample.Application.Customers.CreateCustomer;
using SolidApiExample.Application.Customers.DeleteCustomer;
using SolidApiExample.Application.Customers.GetCustomer;
using SolidApiExample.Application.Customers.ListCustomers;
using SolidApiExample.Application.Customers.UpdateCustomer;
using SolidApiExample.Application.Pipeline;
using SolidApiExample.Application.Repositories;
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

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
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

        services.AddSingleton<ICustomersRepo, InMemoryCustomersRepo>();
        services.AddSingleton<IOrdersRepo, InMemoryOrdersRepo>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateCustomerHandler>());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddScoped<IValidator<GetCustomerQuery>, GetCustomerValidator>();
        services.AddScoped<IValidator<ListCustomersQuery>, ListCustomersValidator>();
        services.AddScoped<IValidator<CreateCustomerCommand>, CreateCustomerValidator>();
        services.AddScoped<IValidator<UpdateCustomerCommand>, UpdateCustomerValidator>();
        services.AddScoped<IValidator<DeleteCustomerCommand>, DeleteCustomerValidator>();

        services.AddScoped<IValidator<GetOrderQuery>, GetOrderValidator>();
        services.AddScoped<IValidator<ListOrdersQuery>, ListOrdersValidator>();
        services.AddScoped<IValidator<CreateOrderCommand>, CreateOrderValidator>();
        services.AddScoped<IValidator<UpdateOrderCommand>, UpdateOrderValidator>();
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
