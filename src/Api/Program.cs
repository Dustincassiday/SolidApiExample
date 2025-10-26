using SolidApiExample.Application.Contracts;
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
using SolidApiExample.Application.Repositories;
using SolidApiExample.Infrastructure.Repositories.InMemory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ProblemDetailsExceptionHandler>();

// Repos
builder.Services.AddSingleton<IPeopleRepo, InMemoryPeopleRepo>();
builder.Services.AddSingleton<IOrdersRepo, InMemoryOrdersRepo>();

// People handlers
builder.Services.AddScoped<IGetById<Guid, PersonDto>, GetPerson>();
builder.Services.AddScoped<IListItems<PersonDto>, ListPeople>();
builder.Services.AddScoped<ICreate<CreatePersonDto, PersonDto>, CreatePerson>();
builder.Services.AddScoped<IUpdate<Guid, UpdatePersonDto, PersonDto>, UpdatePerson>();
builder.Services.AddScoped<IDelete<Guid>, DeletePerson>();

// Orders handlers
builder.Services.AddScoped<IGetById<Guid, OrderDto>, GetOrder>();
builder.Services.AddScoped<IListItems<OrderDto>, ListOrders>();
builder.Services.AddScoped<ICreate<CreateOrderDto, OrderDto>, CreateOrder>();
builder.Services.AddScoped<IUpdate<Guid, UpdateOrderDto, OrderDto>, UpdateOrder>();

var app = builder.Build();

// Configure middleware pipeline
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthorization();

// NOTE: Swagger in dev
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
