using FluentValidation.AspNetCore;
using Base.Api.Application.Extensions;
using Base.Infrastructure.Persistance.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .AddFluentValidation(); // Enabled FluentValidation

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Calling Custom AddInfrastructureRegistration For AddDbContext to Services.
builder.Services.AddInfrastructureRegistration(builder.Configuration);

// Calling Custom AddApplicationRegistration For AutoMapper,FluentValidation and MetiatR services.
builder.Services.AddApplicationRegistration();

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
