using DiscountServer.Services;
using DiscountStorage;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add ef core with SQLite
builder.Services.AddDbContext<DiscountDbContext>(option => option.UseSqlite("Data Source=discounts.db"));

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountServiceImpl>();
app.MapGet("/", () => "gRPC Server running!");

app.Run();

