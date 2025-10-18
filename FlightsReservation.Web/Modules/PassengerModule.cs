using Microsoft.AspNetCore.Mvc;
using Carter;
using FlightsReservation.BLL.Entities.DataTransferObjects.PassengerDtos;
using FlightsReservation.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using FlightsReservation.BLL.Services.EntityServices;

namespace FlightsReservation.Web.Modules;

[Authorize(Roles = "Admin")]
public class PassengerModule() : CarterModule("/Passengers")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/GetPassenger", async (Guid id, PassengersService service, CancellationToken ct = default) =>
        {
            var response = await service.GetPassengerByIdAsync(id, ct);
            return response.ToHttpResult();
        });

        app.MapPut("/UpdatePassenger",
            async ([FromBody] PassengerUpdateDto productUpdateDto, PassengersService service, CancellationToken ct = default) =>
            {
                var response = await service.UpdatePassengerAsync(productUpdateDto, ct);
                return response.ToHttpResult();
            });

        app.MapDelete("/DeletePassenger", async (Guid id, PassengersService service, CancellationToken ct = default) =>
        {
            var response = await service.DeletePassengerAsync(id, ct);
            return response.ToHttpResult();
        });
    }
}