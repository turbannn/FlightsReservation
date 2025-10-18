using Carter;
using FlightsReservation.BLL.Entities.DataTransferObjects.UserDtos;
using FlightsReservation.BLL.Services.EntityServices;
using FlightsReservation.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightsReservation.Web.Modules;

public class UserModule() : CarterModule("/Users")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/GetUser", async (Guid id, UsersService service, CancellationToken ct = default) =>
        {
            var response = await service.GetUserByIdAsync(id, ct);
            return response.ToHttpResult();
        }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });

        app.MapPost("/AddUser",
            async ([FromBody] UserCreateDto createDto, UsersService service, CancellationToken ct = default) =>
            {
                var response = await service.AddUserAsync(createDto, ct);
                return response.ToHttpResult();
            });

        app.MapPut("/UpdateUser",
            async ([FromBody] UserUpdateDto productUpdateDto, UsersService service, CancellationToken ct = default) =>
            {
                var response = await service.UpdateUserAsync(productUpdateDto, ct);
                return response.ToHttpResult();
            });

        app.MapDelete("/DeleteUser", async (Guid id, UsersService service, CancellationToken ct = default) =>
        {
            var response = await service.DeleteUser(id, ct);
            return response.ToHttpResult();
        });
    }
}