using FlightsReservation.BLL.DtoEntities.FlightDtos;
using FlightsReservation.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Carter;

namespace FlightsReservation.Web.Modules;

public class FlightModule : CarterModule
{
    public FlightModule() : base("/Flights")
    {

    }
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/GetFlight", async (Guid id, FlightsService service) =>
        {
            var response = await service.GetFlightByIdAsync(id);
            return Results.Ok(response);
        });

        app.MapPost("/AddFlight",
            async ([FromBody] FlightCreateDto createDto, FlightsService service) =>
            {
                await service.AddFlightAsync(createDto);
                return Results.Ok();
            });

        app.MapPut("/UpdateFlight",
            async ([FromBody] FlightUpdateDto productUpdateDto, FlightsService service) =>
            {
                await service.UpdateFlightAsync(productUpdateDto);
                return Results.Ok();
            });

        app.MapDelete("/DeleteFlight", async (Guid id, FlightsService service) =>
        {
            await service.DeleteFlight(id);
            return Results.Ok();
        });
    }
}