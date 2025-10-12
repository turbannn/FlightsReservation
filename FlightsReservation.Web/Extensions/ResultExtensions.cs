using FlightsReservation.BLL.Entities.Utilities.Results;

namespace FlightsReservation.Web.Extensions;

public static class ResultExtensions
{
    public static IResult ToHttpResult<T>(this FlightReservationResult<T> result)
    {
        if (result.IsSuccess)
            return Results.Ok(result);

        switch (result.Code)
        {
            case 400:
                return Results.BadRequest(result);

            case 401:
                return Results.Unauthorized();

            case 403:
                return Results.Forbid();

            case 404:
                return Results.NotFound(result);

            case 500:
                return Results.BadRequest(result);
        }

        return Results.BadRequest(result);
    }

    public static IResult ToHttpResult<T>(this FlightReservationPagedResult<T> result)
    {
        if (result.IsSuccess)
            return Results.Ok(result);

        switch (result.Code)
        {
            case 400:
                return Results.BadRequest(result);

            case 401:
                return Results.Unauthorized();

            case 403:
                return Results.Forbid();

            case 404:
                return Results.NotFound(result);

            case 500:
                return Results.BadRequest(result);
        }

        return Results.BadRequest(result);
    }
}