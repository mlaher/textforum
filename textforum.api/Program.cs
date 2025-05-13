using Microsoft.Extensions.Configuration;
using textforum.data;
using textforum.domain.interfaces;
using textforum.domain.models;
using textforum.logic.filters;
using textforum.logic.services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDataLayer(builder.Configuration);
builder.Services.Configure<ValidApps>(builder.Configuration.GetSection("ValidApps"));
builder.Services.AddSingleton<IAppAuthenticationService, AppAuthenticationService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserAuthenticationService, UserAuthenticationService>();

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
