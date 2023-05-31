using Microsoft.EntityFrameworkCore;
using Writing.Configurations;
using Writing.Payloads.Converters;
using Writing.Payloads.DTOs;
using Writing.Payloads.Responses;
using Writing.Repositories;
using Writing.Services;
using Writing.Services.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<UserConverter>();
builder.Services.AddSingleton<SecurityConfiguration>();
builder.Services.AddSingleton<ResponseObject<UserDTO>>();
builder.Services.AddTransient<AuthService, AuthServiceImpl>();
builder.Services.AddDbContext<DataContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();