using Carter;
using FlightsReservation.BLL.Entities.DataTransferObjects.FlightDtos;
using FlightsReservation.BLL.Entities.DataTransferObjects.SeatDtos;
using FlightsReservation.BLL.Services;

namespace FlightsReservation.Web.Modules;

public class UtilityModule() : CarterModule("/Flights")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/Refresh", async (string password, SeatsService seatsService, FlightsService flightsService, CancellationToken ct) =>
        {
            if(password != "qwer123")
                return Results.Unauthorized();

            await flightsService.DeleteAllFlights(ct);

            var random = new Random();
            var airports = new[] { "Warsaw", "Berlin", "Paris", "London", "Rome", "Prague" };

            int n = 20;

            for (int i = 0; i < n; i++)
            {
                var departure = airports[random.Next(airports.Length)];
                var arrival = airports.Except(new[] { departure }).OrderBy(_ => random.Next()).First();

                var depDay = DateTime.UtcNow.AddDays(random.Next(1, 30));

                var createFlightDto = new FlightCreateDto
                {
                    FlightNumber = $"FL{random.Next(1000, 9999)}",
                    Departure = departure,
                    Arrival = arrival,
                    DepartureTime = depDay,
                    ArrivalTime = depDay.AddHours(random.Next(1, 4)),
                    AirplaneType = $"Boeing {random.Next(700, 799)}"
                };

                var flightResult = await flightsService.AddFlightAsync(createFlightDto, ct);
                if (!flightResult.IsSuccess)
                    return Results.BadRequest();
            }

            var flightsRead = await flightsService.GetFlightPageAsync(1, n, ct);

            if (flightsRead.Value is null)
                return Results.NotFound();

            foreach (var f in flightsRead.Value)
            {
                // 3. Seats
                var seatCount = random.Next(5, 10);
                for (int j = 1; j <= seatCount; j++)
                {
                    var seatDto = new SeatCreateDto
                    {
                        SeatNumber = $"{(char)('A' + j - 1)}{random.Next(1, 30)}",
                        FlightId = f.Id
                    };
                    await seatsService.AddSeatAsync(seatDto, ct);
                }
            }


            return Results.Ok("Refreshed");
        });
    }
}