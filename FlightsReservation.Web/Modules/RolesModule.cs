using FlightsReservation.BLL.Services;
using Carter;
using FlightsReservation.BLL.Entities.Utilities.Other;
using Microsoft.AspNetCore.Mvc;

namespace FlightsReservation.Web.Modules;

public class RolesModule() : CarterModule("/Rights")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/GainAdminRights",
            ([FromBody] AdminLogin adminLogin, TokenService service, HttpResponse response, CancellationToken ct = default) =>
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