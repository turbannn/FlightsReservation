using Microsoft.AspNetCore.Mvc;
using Carter;
using FlightsReservation.BLL.Entities.DataTransferObjects.SeatDtos;
using FlightsReservation.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using FlightsReservation.BLL.Services.EntityServices;

namespace FlightsReservation.Web.Modules;

[Authorize(Roles = "Admin")]
public class SeatModule() : CarterModule("/Seats")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/GetSeat", async (Guid id, SeatsService service, CancellationToken ct = default) =>
        {
            var response = await service.GetSeatByIdAsync(id, ct);
            return response.ToHttpResult();
        });

        app.MapPost("/AddSeat",
            async ([FromBody] SeatCreateDto createDto, SeatsService service, CancellationToken ct = default) =>
            {
                var response = await service.AddSeatAsync(createDto, ct);
                return response.ToHttpResult();
            });

        app.MapPut("/UpdateSeat",
            async ([FromBody] SeatUpdateDto productUpdateDto, SeatsService service, CancellationToken ct = default) =>
            {
                var response = await service.UpdateSeatAsync(productUpdateDto, ct);
                return response.ToHttpResult();
            });

        app.MapDelete("/DeleteSeat", async (Guid id, SeatsService service, CancellationToken ct = default) =>
        {
            var response = await service.DeleteSeat(id, ct);
            return response.ToHttpResult();
        });
    }
}