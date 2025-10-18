using Carter;
using FlightsReservation.BLL.Services.UtilityServices;
using FlightsReservation.Web.Extensions;

namespace FlightsReservation.Web.Modules;

public class UtilityModule() : CarterModule("/Utils")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/RefreshFlights", async (string password, RefreshService service, CancellationToken ct) =>
        {
            var res = await service.RefreshDatabaseAsync(password, ct);
            return res.ToHttpResult();
        });
    }
}