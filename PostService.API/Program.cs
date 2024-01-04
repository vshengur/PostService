using Mapster;
using MapsterMapper;

using Microsoft.EntityFrameworkCore;
using PostService.API.Services;
using PostService.Domain.DbContexts;
using PostService.Domain.Interfaces;
using PostService.Domain.Models;
using PostService.Infrastructure.Caching;

using StackExchange.Redis;

using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IMapper, ServiceMapper>();

// Register Mapster
var config = TypeAdapterConfig.GlobalSettings;
config.Scan(Assembly.GetExecutingAssembly());

builder.Services.AddSingleton(config);

// Register AppDbContext
var sqlConnectionString = builder.Configuration.GetConnectionString("DbConnection");
builder.Services.AddDbContextPool<AppDbContext>(op =>
    op.UseSqlServer(sqlConnectionString)
);

// Register Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(opt =>
  ConnectionMultiplexer.Connect(
    builder.Configuration.GetConnectionString("RedisCacheURL")!
  )
);

// Add services to the container.
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddSingleton<ICacheService<Post>>(provider =>
    new CacheService<Post>(provider.GetService<IConnectionMultiplexer>()!, 1));
builder.Services.AddScoped<IPostService, PostService.API.Services.PostService>();

builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
