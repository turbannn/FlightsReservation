using FlightsReservation.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Carter;
using FlightsReservation.BLL.Entities.DataTransferObjects.FlightDtos;
using FlightsReservation.Web.Extensions;

namespace FlightsReservation.Web.Modules;

public class FlightModule() : CarterModule("/Flights")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/GetFlight", async (Guid id, FlightsService service, CancellationToken ct = default) =>
        {
            var response = await service.GetFlightByIdAsync(id, ct);
            return response.ToHttpResult();
        });

        app.MapPost("/AddFlight",
            async ([FromBody] FlightCreateDto createDto, FlightsService service, CancellationToken ct = default) =>
            {
                var response = await service.AddFlightAsync(createDto, ct);
                return response.ToHttpResult();
            });

        app.MapPut("/UpdateFlight",
            async ([FromBody] FlightUpdateDto productUpdateDto, FlightsService service, CancellationToken ct = default) =>
            {
                var response = await service.UpdateFlightAsync(productUpdateDto, ct);
                return response.ToHttpResult();
            });

        app.MapDelete("/DeleteFlight", async (Guid id, FlightsService service, CancellationToken ct = default) =>
        {
            var response = await service.DeleteFlight(id, ct);
            return response.ToHttpResult();
        });
    }
}