using Carter;
using FlightsReservation.BLL.Entities.DataTransferObjects.UserDtos;
using FlightsReservation.BLL.Entities.Utilities.Results;
using FlightsReservation.BLL.Services.EntityServices;
using FlightsReservation.BLL.Services.UtilityServices;
using FlightsReservation.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightsReservation.Web.Modules;

public class UserModule() : CarterModule("/Users")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/GetUserProfile", async (Guid id, UsersService service, CancellationToken ct = default) =>
        {
            var response = await service.GetUserProfileByIdAsync(id, ct);
            return response.ToHttpResult();
        }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,User" });

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
            async (string login, string password, HttpResponse res, UsersService service, TokenService tokenService, CancellationToken ct = default) =>
            {
                var response = await service.LoginAsync(login, password, ct);

                if (!response.IsSuccess)
                    return response.ToHttpResult();

                var tokenstr = tokenService.CreateAccessToken(response.Value!.Id, response.Value.Role);

                res.Cookies.Append("_t", tokenstr, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.Now.AddMinutes(15)
                });

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