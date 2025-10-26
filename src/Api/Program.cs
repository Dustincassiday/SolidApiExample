using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SolidApiExample.Api.Errors;
using SolidApiExample.Application.Contracts;
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
using SolidApiExample.Application.Repositories;
using SolidApiExample.Infrastructure.Repositories.InMemory;

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
        ConfigureServices(builder.Services);

        var app = builder.Build();
        ConfigurePipeline(app);
        return app;
    }

    internal static void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddProblemDetails();
        services.AddExceptionHandler<ProblemDetailsExceptionHandler>();

        services.AddSingleton<IPeopleRepo, InMemoryPeopleRepo>();
        services.AddSingleton<IOrdersRepo, InMemoryOrdersRepo>();

        services.AddScoped<IGetById<Guid, PersonDto>, GetPerson>();
        services.AddScoped<IListItems<PersonDto>, ListPeople>();
        services.AddScoped<ICreate<CreatePersonDto, PersonDto>, CreatePerson>();
        services.AddScoped<IUpdate<Guid, UpdatePersonDto, PersonDto>, UpdatePerson>();
        services.AddScoped<IDelete<Guid>, DeletePerson>();

        services.AddScoped<IGetById<Guid, OrderDto>, GetOrder>();
        services.AddScoped<IListItems<OrderDto>, ListOrders>();
        services.AddScoped<ICreate<CreateOrderDto, OrderDto>, CreateOrder>();
        services.AddScoped<IUpdate<Guid, UpdateOrderDto, OrderDto>, UpdateOrder>();
    }

    internal static void ConfigurePipeline(WebApplication app)
    {
        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapControllers();
    }
}
