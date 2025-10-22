using Carter;
using FlightsReservation.BLL.Entities.DataTransferObjects.UserDtos;
using FlightsReservation.BLL.Entities.Utilities.Results;
using FlightsReservation.BLL.Services.EntityServices;
using FlightsReservation.BLL.Services.UtilityServices.Authentication;
using FlightsReservation.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlightsReservation.Web.Modules;

public class UserModule() : CarterModule("/Users")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/GetUserProfile", async (HttpContext http, UsersService service, CancellationToken ct = default) =>
        {
            //3d781d10-d652-464c-82de-9b7d667549bf - test user id
            /*
            var idStr = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(idStr) || !Guid.TryParse(idStr, out Guid userId))
                return Results.BadRequest(new { success = false, message = "Invalid user ID" }); 
            */
            var userId = Guid.Parse("3d781d10-d652-464c-82de-9b7d667549bf");
            var response = await service.GetUserProfileByIdAsync(userId, ct);
            return response.ToHttpResult();
        });//.RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,User" }); //- for test without token

        app.MapGet("/GetUser", async (Guid id, UsersService service, CancellationToken ct = default) =>
        {
            var response = await service.GetUserByIdAsync(id, ct);
            return response.ToHttpResult();
        }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });

        app.MapPost("/CommitRegistration",
            async ([FromBody] UserCreateDto createDto, UsersService service, CancellationToken ct = default) =>
            {
                var response = await service.AddUserAsync(createDto, ct);
                return response.ToHttpResult();
            });

        app.MapGet("/CommitLogin",
            async (string login, string password, HttpResponse res, HttpContext context, UsersService service, TokenService tokenService, CancellationToken ct = default) =>
            {
                var response = await service.LoginAsync(login, password, ct);

                if (!response.IsSuccess)
                    return response.ToHttpResult();

                var tokenstr = tokenService.CreateAccessToken(response.Value!.Id, response.Value.Role);
                Console.WriteLine("Generated Token: " + tokenstr);
                Console.WriteLine("Request Origin: " + context.Request.Headers["Origin"]);
                
#if DEBUG
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.Now.AddMinutes(15),
                    Path = "/",
                    Domain = null
                };
                
                res.Cookies.Append("_t", tokenstr, cookieOptions);
                Console.WriteLine("Cookie set with options: HttpOnly=true, Secure=false, SameSite=Lax, Path=/, Domain=null");
#else
                res.Cookies.Append("_t", tokenstr, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // HTTPS для production
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.Now.AddMinutes(15),
                    Path = "/"
                });
#endif

                return response.ToHttpResult();
            });

        app.MapPost("/CommitLogout",
            async (HttpResponse res, CancellationToken ct = default) =>
            {
                res.Cookies.Delete("_t");
                return await Task.FromResult(FlightReservationResult<bool>.Success(true, 200));
            });

        app.MapPut("/UpdateUser",
            async ([FromBody] UserUpdateDto productUpdateDto, UsersService service, CancellationToken ct = default) =>
            {
                var response = await service.UpdateUserAsync(productUpdateDto, ct);
                return response.ToHttpResult();
            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,User" });

        app.MapPut("/UpdateUserMoney",
            async (Guid id, int amount, UsersService service, CancellationToken ct = default) =>
            {
                var response = await service.UpdateUserMoneyAsync(id, amount, ct);
                return response.ToHttpResult();
            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });

        app.MapDelete("/DeleteUser", async (Guid id, UsersService service, CancellationToken ct = default) =>
        {
            var response = await service.DeleteUser(id, ct);
            return response.ToHttpResult();
        }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,User" }); ;
    }
}