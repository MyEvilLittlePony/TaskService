using Microsoft.EntityFrameworkCore;
using TaskService;
using TaskService.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<TaskDbContext>(opt =>
    opt.UseInMemoryDatabase("Tasks"));

builder.Services.AddScoped<TaskProcessor>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
