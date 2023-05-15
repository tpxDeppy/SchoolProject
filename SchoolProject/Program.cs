global using Microsoft.EntityFrameworkCore;
using SchoolProject.Data;
using FluentValidation;
using SchoolProject.Services.Validation;
using System.Text.Json.Serialization;
using SchoolProject.Services.Interfaces;
using SchoolProject.Services.Implementations;
using SchoolProject.Models.Entities;

var builder = WebApplication.CreateBuilder(args);

//Enabling CORS in order to be able to fetch the API in the frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowTrustedOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Add services to the container.

builder.Services.AddControllers();

//Fixing error: 'A possible object cycle was detected' from GetPeopleInClass
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Registering AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<ISchoolService, SchoolService>();
builder.Services.AddScoped<IClassService, ClassService>();
//Registering Validator for Person
builder.Services.AddScoped<IValidator<Person>, PersonValidator>();

//Dependency Injection for DbContext
builder.Services.AddDbContext<DataContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();    
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowTrustedOrigins");

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
    await next();
});

app.MapControllers();

app.Run();
