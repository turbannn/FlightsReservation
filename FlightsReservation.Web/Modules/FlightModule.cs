using FlightsReservation.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Carter;
using FlightsReservation.BLL.Entities.DataTransferObjects.FlightDtos;
using FlightsReservation.BLL.Entities.Utilities.Requests;
using FlightsReservation.Web.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace FlightsReservation.Web.Modules;

public class FlightModule() : CarterModule("/Flights")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/RequestFlightsPageWithReturn", async (int page,
            int size,
            FlightSearchWithReturnRequest request,
            FlightsService service,
            CancellationToken ct = default) =>
        {
            var response = await service.GetFlightPageFromRequestWithReturnAsync(page, size, request, ct);
            return response.ToHttpResult();
        });

        app.MapPost("/RequestFlightsPage", async (int page,
            int size,
            FlightSearchRequest request,
            FlightsService service,
            CancellationToken ct = default) =>
        {
            var response = await service.GetFlightPageFromRequestAsync(page, size, request, ct);
            return response.ToHttpResult();
        });

        app.MapGet("/GetFlightsPage", async (int page, int size, FlightsService service, CancellationToken ct = default) =>
        {
            var response = await service.GetFlightPageAsync(page, size, ct);
            return response.ToHttpResult();
        }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });

        app.MapGet("/GetFlight", async (Guid id, FlightsService service, CancellationToken ct = default) =>
        {
            var response = await service.GetFlightByIdAsync(id, ct);
            return response.ToHttpResult();
        }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });

        app.MapPost("/AddFlight",
            async ([FromBody] FlightCreateDto createDto, FlightsService service, CancellationToken ct = default) =>
            {
                var response = await service.AddFlightAsync(createDto, ct);
                return response.ToHttpResult();
            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });

        app.MapPut("/UpdateFlight",
            async ([FromBody] FlightUpdateDto productUpdateDto, FlightsService service, CancellationToken ct = default) =>
            {
                var response = await service.UpdateFlightAsync(productUpdateDto, ct);
                return response.ToHttpResult();
            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });

        app.MapDelete("/DeleteFlight", async (Guid id, FlightsService service, CancellationToken ct = default) =>
        {
            var response = await service.DeleteFlight(id, ct);
            return response.ToHttpResult();
        }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });
    }
}