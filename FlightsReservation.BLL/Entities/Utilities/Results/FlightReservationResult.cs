namespace FlightsReservation.BLL.Entities.Utilities.Results;

public class FlightReservationResult<TResultType>
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public TResultType? Value { get; init; }
    public int Code { get; init; }

    public static FlightReservationResult<TResultType> Success(TResultType value, int code)
        => new() { IsSuccess = true, Value = value, Code = code };

    public static FlightReservationResult<TResultType> Fail(string error, int code)
        => new() { IsSuccess = false, ErrorMessage = error, Code = code };
}

