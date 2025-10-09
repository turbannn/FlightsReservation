using FlightsReservation.BLL.MapperProfiles;
using FlightsReservation.BLL.Services;
using FlightsReservation.BLL.Validators;
using FlightsReservation.DAL.Data;
using FlightsReservation.DAL.Interfaces;
using FlightsReservation.DAL.Repositories;
using FlightsReservation.DAL.UoWs;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Carter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<FlightsDbContext>(options => options.UseNpgsql(connection));

// Add services to the container.
//Repositories
builder.Services.AddScoped<ISeatsRepository, SeatsRepository>();
builder.Services.AddScoped<IPassengersRepository, PassengersRepository>();
builder.Services.AddScoped<IReservationsRepository, ReservationRepository>();
builder.Services.AddScoped<IFlightsRepository, FlightsRepository>();

//UoW
builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();

//OOM Mapper
builder.Services.AddAutoMapper(cfg => { }, typeof(SeatProfile).Assembly);

//Validators
builder.Services.AddValidatorsFromAssembly(typeof(FlightDtoValidator).Assembly);

//Services
builder.Services.AddScoped<SeatsService>();
builder.Services.AddScoped<PassengersService>();
builder.Services.AddScoped<ReservationsService>();
builder.Services.AddScoped<FlightsService>();

//Routing modules
builder.Services.AddCarter();

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

app.MapCarter();

app.Run();