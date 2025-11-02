using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Carter;
using FlightsReservation.BLL.Entities.DataTransferObjects.ReservationDtos;
using FlightsReservation.BLL.Entities.Utilities.Other;
using FlightsReservation.BLL.Entities.Utilities.Results;
using FlightsReservation.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using FlightsReservation.BLL.Services.EntityServices;

namespace FlightsReservation.Web.Modules;

public class ReservationModule() : CarterModule("/Reservations")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/GetReservation", async (Guid id, ReservationsService service, CancellationToken ct = default) =>
        {
            var response = await service.GetReservationByIdAsync(id, ct);
            return response.ToHttpResult();
        }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });

        app.MapPost("/CommitReservation",
            async ([FromBody] ReservationCreateDto createDto, HttpContext http, ReservationsService service,
                CancellationToken ct = default) =>
            {
                var idStr = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(idStr) || !Guid.TryParse(idStr, out Guid userId))
                    return Results.BadRequest(new { success = false, message = "Invalid user ID" });
                
                var response = await service.CommitReservationAsync(userId, createDto, ct);
                return response.ToHttpResult();
            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,User" });

        app.MapDelete("/DeleteReservation", async (Guid id, ReservationsService service, CancellationToken ct = default) =>
        {
            var response = await service.DeleteReservation(id, ct);
            return response.ToHttpResult();
        }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });

        app.MapPost("/BeginReservation", async ([FromBody]List<Guid> ids, HttpContext con, SeatsService service, CancellationToken ct = default) =>
        {
            con.Request.Cookies.TryGetValue("_ids", out string? existingIds);

            if (existingIds is not null)
                return FlightReservationResult<int>.Fail("Seats already occupied", ResponseCodes.BadRequest).ToHttpResult();
            
            var response = await service.LockSeats(ids, ct);

            if (!response.IsSuccess)
                return response.ToHttpResult();
            
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.Now.AddMinutes(15),
                Path = "/",
            };
            
            string idString = string.Join(",", ids);

            con.Response.Cookies.Append("_ids", idString, cookieOptions);

            return response.ToHttpResult();
        }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,User" });
    }
}