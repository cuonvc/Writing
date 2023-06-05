using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Writing.Configurations;
using Writing.Enumerates;
using Writing.Payloads.Converters;
using Writing.Payloads.DTOs;
using Writing.Payloads.Responses;
using Writing.Repositories;
using Writing.Services;
using Writing.Services.Impl;
using Action = Writing.Enumerates.Action;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = "localhost";
    options.InstanceName = "sampleInstance";
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<UserConverter>();
builder.Services.AddSingleton<PostConverter>();
builder.Services.AddSingleton<CategoryConverter>();
builder.Services.AddSingleton<SecurityConfiguration>();
builder.Services.AddSingleton<ResponseObject<UserDTO>>();
builder.Services.AddSingleton<ResponseObject<List<UserDTO>>>();
builder.Services.AddSingleton<ResponseObject<List<CategoryDTO>>>();
builder.Services.AddSingleton<ResponseObject<Action>>();
builder.Services.AddSingleton<ResponseObject<PostDTO>>();
builder.Services.AddSingleton<ResponseObject<CategoryDTO>>();
builder.Services.AddSingleton<ResponseObject<string>>();
builder.Services.AddSingleton<ResponseData<ActionStatus>>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<ResponseObject<ResponseTokenObject>>();
builder.Services.AddTransient<AuthService, AuthServiceImpl>();
builder.Services.AddTransient<RefreshTokenService, RefreshTokenImpl>();
builder.Services.AddTransient<UserService, UserServiceImpl>();
builder.Services.AddTransient<RelationshipService, RelationshipServiceImpl>();
builder.Services.AddTransient<PostService, PostServiceImpl>();
builder.Services.AddTransient<CategoryService, CategoryServiceImpl>();
builder.Services.AddTransient<CommentService, CommentServiceImpl>();
builder.Services.AddDbContext<DataContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("OtherConnection"));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration.GetSection("Jwt:Secret-key").Value!))
    };
});

builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme {
        Description = "Jwt Authorization",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
    });
    
    options.OperationFilter<SecurityRequirementsOperationFilter>();
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