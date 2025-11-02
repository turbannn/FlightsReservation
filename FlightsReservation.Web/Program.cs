using System.Security.Claims;
using System.Text;
using FlightsReservation.BLL.MapperProfiles;
using FlightsReservation.DAL.Data;
using FlightsReservation.DAL.Interfaces;
using FlightsReservation.DAL.Repositories;
using FlightsReservation.DAL.UoWs;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Carter;
using FlightsReservation.DAL.Entities.Utils.Authentication;
using FlightsReservation.DAL.Entities.Utils.Email;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using QuestPDF.Infrastructure;
using FlightsReservation.BLL.Interfaces.Services;
using FlightsReservation.BLL.Services.EntityServices;
using FlightsReservation.BLL.Services.UtilityServices;
using FlightsReservation.DAL.Entities.Utils.Payment;
using FlightsReservation.BLL.Services.UtilityServices.FilnalizeReservation;
using FlightsReservation.BLL.Services.UtilityServices.Simulation;
using FlightsReservation.BLL.Services.UtilityServices.Authentication;
using FlightsReservation.BLL.Services.UtilityServices.Payment;
using FlightsReservation.BLL.Validators.DtoEntitiesValidators;
using FlightsReservation.BLL.Services.UtilityServices.FlightsApi;
using FlightsReservation.DAL.Entities.Utils.FlightsApiSettings;

//Licenses
QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connection = builder.Configuration.GetConnectionString("DefaultConnection");

//Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins(
                "http://localhost:63342",   // WebStorm
                "http://127.0.0.1:63342",   // Alternative localhost
                "http://localhost:5500",    // Live Server (if used)
                "http://127.0.0.1:5500"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowedToAllowWildcardSubdomains();
    });
});

builder.Services.AddDbContext<FlightsDbContext>(options => options.UseNpgsql(connection));

// Add services to the container.
//Repositories
builder.Services.AddScoped<ISeatsRepository, SeatsRepository>();
builder.Services.AddScoped<IPassengersRepository, PassengersRepository>();
builder.Services.AddScoped<IReservationsRepository, ReservationRepository>();
builder.Services.AddScoped<IFlightsRepository, FlightsRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();

//UoW
builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();

var emailSettings = builder.Configuration
    .GetSection("EmailSettings")
    .Get<EmailSettings>();

if(emailSettings is null)
    throw new Exception("Email settings are not configured properly");

var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>();

if (jwtSettings is null)
    throw new Exception("Email settings are not configured properly");

var payuSettings = builder.Configuration
    .GetSection("PayU")
    .Get<PayuSettings>();

if (payuSettings is null)
    throw new Exception("PayU settings are not configured properly");

var aviationStackSettings = builder.Configuration
    .GetSection("AviationStackSettings")
    .Get<AviationStackSettings>();

if (aviationStackSettings is null)
    throw new Exception("AviationStack settings are not configured properly");

//Settings
builder.Services.AddSingleton(emailSettings);
builder.Services.AddSingleton(jwtSettings);
builder.Services.AddSingleton(payuSettings);
builder.Services.AddSingleton(aviationStackSettings);

//OOM Mapper
builder.Services.AddAutoMapper(cfg => { }, typeof(SeatProfile).Assembly);

//Validators
builder.Services.AddValidatorsFromAssembly(typeof(FlightDtoValidator).Assembly);

//Http Client
builder.Services.AddHttpClient("PayU", client =>
{
    client.BaseAddress = new Uri(payuSettings.BaseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

//Services
builder.Services.AddScoped<SeatsService>();
builder.Services.AddScoped<PassengersService>();
builder.Services.AddScoped<ReservationsService>();
builder.Services.AddScoped<FlightsService>();
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<IEmailService, MailkitEmailService>();
builder.Services.AddScoped<IPdfService, QuestPdfService>();
builder.Services.AddScoped<TokenService>();

builder.Services.AddScoped<AviationStackService>();

builder.Services.AddScoped<RefreshService>();
builder.Services.AddScoped<PayuService>();

//Routing modules
builder.Services.AddCarter();

//Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            RoleClaimType = ClaimTypes.Role
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.HttpContext.Request.Cookies["_t"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

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

app.UseCors("AllowLocalhost");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapCarter();

app.Run();