using SolidApiExample.Application.Contracts;
using SolidApiExample.Application.Orders.DTOs;
using SolidApiExample.Application.Orders.Handlers;
using SolidApiExample.Application.People.DTOs;
using SolidApiExample.Application.People.Handlers;
using SolidApiExample.Application.Repositories;
using SolidApiExample.Infrastructure.Repositories.InMemory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
app.UseHttpsRedirection();
app.UseAuthorization();

// NOTE: Swagger in dev
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
