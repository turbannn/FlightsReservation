using Carter;
using FlightsReservation.BLL.Entities.Utilities.Other;
using FlightsReservation.BLL.Services.UtilityServices.Authentication;
using FlightsReservation.BLL.Services.UtilityServices.Simulation;
using FlightsReservation.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightsReservation.Web.Modules;

public class UtilityModule() : CarterModule("/Utils")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/RefreshFlights", async (string password, RefreshService service, CancellationToken ct) =>
        {
            var res = await service.RefreshDatabaseAsync(password, ct);

            return res.ToHttpResult();
        }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" }); ;

        app.MapPost("/GainAdminRights",
            ([FromBody] AdminLogin adminLogin, TokenService service, HttpResponse response,
                CancellationToken ct = default) =>
            {
                if (adminLogin.Login != "qwer123" || adminLogin.Password != "qwer123")
                    return Results.Unauthorized();

                var tokenstr = service.CreateAccessToken(Guid.NewGuid(), "Admin");

                response.Cookies.Append("_t", tokenstr, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.Now.AddMinutes(15)
                });

                return Results.Ok();
            });

    }
}