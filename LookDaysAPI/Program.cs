using LookDaysAPI.Models;
using Microsoft.EntityFrameworkCore;
using LookDaysAPI.Controllers;

var builder = WebApplication.CreateBuilder(args);
// �K�[ CORS �t�m
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<LookdaysContext>(
    option => option.UseSqlServer(
        builder.Configuration.GetConnectionString("LookDaysConnection")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapForumPostEndpoints();

app.Run();
