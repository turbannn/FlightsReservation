using Microsoft.AspNetCore.Mvc;
using Carter;
using FlightsReservation.BLL.DtoEntities.SeatDtos;
using FlightsReservation.BLL.Services;

namespace FlightsReservation.Web.Modules;

public class SeatModule : CarterModule
{
    public SeatModule() : base("/Seats")
    {

    }
    public override void AddRoutes(IEndpointRouteBuilder app)
    {

        app.MapGet("/GetSeat", async (Guid id, SeatsService service) =>
        {
            var response = await service.GetSeatByIdAsync(id);
            return Results.Ok(response);
        });

        app.MapPost("/AddSeat",
            async ([FromBody] SeatCreateDto createDto, SeatsService service) =>
            {
                await service.AddSeatAsync(createDto);
                return Results.Ok();
            });

        app.MapPut("/UpdateSeat",
            async ([FromBody] SeatUpdateDto productUpdateDto, SeatsService service) =>
            {
                await service.UpdateSeatAsync(productUpdateDto);
                return Results.Ok();
            });

        app.MapDelete("/DeleteSeat", async (Guid id, SeatsService service) =>
        {
            await service.DeleteSeat(id);
            return Results.Ok();
        });
    }
}