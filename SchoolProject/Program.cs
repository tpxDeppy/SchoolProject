global using Microsoft.EntityFrameworkCore;
global using SchoolProject.DAL;
using FluentValidation;
using SchoolProject.API.Services.ClassService;
using SchoolProject.API.Services.PersonService;
using SchoolProject.API.Services.SchoolService;
using SchoolProject.API.Services.Validation;
using SchoolProject.BL.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Fixing error: 'A possible object cycle was detected' from GetPeopleInClass
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Registering AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
