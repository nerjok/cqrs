using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;
using Post.Query.Infrastructure.Repositories;
using Post.Query.Infrastructure.Handlers;
using Confluent.Kafka;
using Post.Query.Infrastructure.Consumers;
using CQRS.Core.Consumers;
using Post.Query.Api.Queries;
using Post.Query.Infrastructure.Dispatchers;
using CQRS.Core.Infrastructure;
using Post.Query.Domain.Entities;
using Post.Query.Infrastructure.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

// Action<DbContextOptionsBuilder> configureDBContext = (o => o.UseLazyLoadingProxies().UseSqlite(builder.Configuration.GetConnectionString("SqlServer")));
Action<DbContextOptionsBuilder> configureDBContext = (o => o.UseLazyLoadingProxies().UseSqlite(builder.Configuration.GetConnectionString("WebApiDatabase")));

builder.Services.AddDbContext<DatabaseContext>(configureDBContext);
builder.Services.AddSingleton<DatabaseContextFactory>(new DatabaseContextFactory(configureDBContext));

// CreateTablesFromCode
var dataContext = builder.Services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
dataContext.Database.EnsureCreated();

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentsRepository, CommentRepository>();
builder.Services.AddScoped<IQueryHandler, QueryHandler>();

builder.Services.AddScoped<IEventHandler, Post.Query.Infrastructure.Handlers.EventHandler>();

// Kafke
builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));
builder.Services.AddScoped<IEventConsumer, EventConsumer>();
builder.Services.AddHostedService<ConsumersHostedService>();



// register query handler methods
var queryHandler = builder.Services.BuildServiceProvider().GetRequiredService<IQueryHandler>();
var dispatcher = new QueryDispatcher();
dispatcher.RegisterHandler<FindAllPostsQuery>(queryHandler.HandleAsync);
dispatcher.RegisterHandler<FindPostByIdQuery>(queryHandler.HandleAsync);
dispatcher.RegisterHandler<FindPostsByAuthorQuery>(queryHandler.HandleAsync);
dispatcher.RegisterHandler<FindPostsWithCommentsQuery>(queryHandler.HandleAsync);
dispatcher.RegisterHandler<FindPostsWithLikesQuery>(queryHandler.HandleAsync);
builder.Services.AddSingleton<IQueryDispatcher<PostEntity>>(_ => dispatcher);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(o => o.AddPolicy("AnyOrigin", builder =>
{
    // builder.WithOrigins("*")
    //        .AllowAnyMethod()
    //        .AllowAnyHeader();
    builder
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    .SetIsOriginAllowed((hosts) => true);

}));


var app = builder.Build();

app.MapHub<ChatHub>("/chatHub");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseHttpsRedirection();

app.UseCors("AnyOrigin");

app.Run();
