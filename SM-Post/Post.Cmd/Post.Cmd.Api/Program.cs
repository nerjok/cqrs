using Confluent.Kafka;
using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using Microsoft.EntityFrameworkCore;
using Post.Cmd.Api.Commands;
using Post.Cmd.Domain.Aggregates;
using Post.Cmd.Infrastructure.DataAccess;
using Post.Cmd.Infrastructure.Dispatchers;
using Post.Cmd.Infrastructure.Producers;
using Post.Cmd.Infrastructure.Repositories;
using Post.Cmd.Infrastructure.Stores;
using Post.Common.Handlers;

var builder = WebApplication.CreateBuilder(args);


// TODO add database
Action<DbContextOptionsBuilder> configureDBContext = (o => o.UseLazyLoadingProxies().UseSqlite(builder.Configuration.GetConnectionString("WebApiDatabase")));
builder.Services.AddDbContext<DataContext>(configureDBContext);

// CreateTablesFromCode
var dataContext = builder.Services.BuildServiceProvider().GetRequiredService<DataContext>();
dataContext.Database.EnsureCreated();

builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));


// DBConfig must be here
builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
builder.Services.AddScoped<IEventProducer, EventProducer>();
builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddScoped<IEventSourcingHander<PostAggregate>, EventSourcingHandler>();
builder.Services.AddScoped<ICommandHandler, CommandHandler>();

// Registre CommandHandlers

var commandHandler = builder.Services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
var dispatcher = new CommandDispatcher();
dispatcher.RegisterHandler<PostCommand>(commandHandler.HandleAsync);
dispatcher.RegisterHandler<EditMessageCommand>(commandHandler.HandleAsync);
dispatcher.RegisterHandler<LikesPostCommand>(commandHandler.HandleAsync);
dispatcher.RegisterHandler<AddCommentCommand>(commandHandler.HandleAsync);
dispatcher.RegisterHandler<EditCommentCommand>(commandHandler.HandleAsync);
dispatcher.RegisterHandler<RemoveCommentCommand>(commandHandler.HandleAsync);
dispatcher.RegisterHandler<DeletePostCommand>(commandHandler.HandleAsync);

builder.Services.AddSingleton<ICommandDispatcher>(_ => dispatcher);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
//     app.UseSwaggerUI(c =>
// {
//     c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
//     c.RoutePrefix = "swagger";
// });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
