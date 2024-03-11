using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;
using Post.Query.Infrastructure.Repositories;
using Post.Query.Infrastructure.Handlers;
using Confluent.Kafka;
using Post.Query.Infrastructure.Consumers;
using CQRS.Core.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Action<DbContextOptionsBuilder> configureDBContext = (o => o.UseLazyLoadingProxies().UseSqlite(builder.Configuration.GetConnectionString("SqlServer")));
Action<DbContextOptionsBuilder> configureDBContext = (o => o.UseLazyLoadingProxies().UseSqlite(builder.Configuration.GetConnectionString("WebApiDatabase")));

builder.Services.AddDbContext<DatabaseContext>(configureDBContext);
builder.Services.AddSingleton<DatabaseContextFactory>(new DatabaseContextFactory(configureDBContext));

// CreateTablesFromCode
var dataContext = builder.Services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
dataContext.Database.EnsureCreated();

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentsRepository, CommentRepository>();
builder.Services.AddScoped<IEventHandler, Post.Query.Infrastructure.Handlers.EventHandler>();

// Kafke
builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));
builder.Services.AddScoped<IEventConsumer, EventConsumer>();
builder.Services.AddHostedService<ConsumersHostedService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
