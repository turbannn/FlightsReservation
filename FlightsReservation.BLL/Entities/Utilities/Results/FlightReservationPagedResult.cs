namespace FlightsReservation.BLL.Entities.Utilities.Results;

public class FlightReservationPagedResult<TResultType> : FlightReservationResult<TResultType>
{
    public int TotalCount { get; set; }

    public static FlightReservationPagedResult<TResultType> PagedSuccess(TResultType value, int totalCount)
        => new() { IsSuccess = true, Value = value, TotalCount = totalCount, Code = 200 };

    public static FlightReservationPagedResult<TResultType> PagedFail(string error, int code)
        => new() { IsSuccess = false, ErrorMessage = error, Code = code, TotalCount = -1 };
}

