using System.Text.Json.Serialization;

namespace FlightsReservation.BLL.Entities.Utilities.Other;

public class AviationStackFlight
{
    [JsonPropertyName("weekday")]
    public string? Weekday { get; set; }

    [JsonPropertyName("departure")]
    public DepartureInfo Departure { get; set; } = null!;

    [JsonPropertyName("arrival")]
    public ArrivalInfo Arrival { get; set; } = null!;

    [JsonPropertyName("aircraft")]
    public AircraftInfo? Aircraft { get; set; }

    [JsonPropertyName("airline")]
    public AirlineInfo Airline { get; set; } = null!;

    [JsonPropertyName("flight")]
    public FlightInfo Flight { get; set; } = null!;

    [JsonPropertyName("codeshared")]
    public CodesharedInfo? Codeshared { get; set; }
}

public class DepartureInfo
{
    [JsonPropertyName("iataCode")]
    public string? IataCode { get; set; }

    [JsonPropertyName("icaoCode")]
    public string? IcaoCode { get; set; }

    [JsonPropertyName("terminal")]
    public string? Terminal { get; set; }

    [JsonPropertyName("gate")]
    public string? Gate { get; set; }

    [JsonPropertyName("scheduledTime")]
    public string? ScheduledTime { get; set; }
}

public class ArrivalInfo
{
    [JsonPropertyName("iataCode")]
    public string? IataCode { get; set; }

    [JsonPropertyName("icaoCode")]
    public string? IcaoCode { get; set; }

    [JsonPropertyName("terminal")]
    public string? Terminal { get; set; }

    [JsonPropertyName("gate")]
    public string? Gate { get; set; }

    [JsonPropertyName("scheduledTime")]
    public string? ScheduledTime { get; set; }
}

public class AircraftInfo
{
    [JsonPropertyName("modelCode")]
    public string? ModelCode { get; set; }

    [JsonPropertyName("modelText")]
    public string? ModelText { get; set; }
}

public class AirlineInfo
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("iataCode")]
    public string? IataCode { get; set; }

    [JsonPropertyName("icaoCode")]
    public string? IcaoCode { get; set; }
}

public class FlightInfo
{
    [JsonPropertyName("number")]
    public string? Number { get; set; }

    [JsonPropertyName("iataNumber")]
    public string? IataNumber { get; set; }

    [JsonPropertyName("icaoNumber")]
    public string? IcaoNumber { get; set; }
}

public class CodesharedInfo
{
    [JsonPropertyName("airline")]
    public AirlineInfo? Airline { get; set; }

    [JsonPropertyName("flight")]
    public FlightInfo? Flight { get; set; }
}

public class AviationStackResponse
{
    [JsonPropertyName("pagination")]
    public PaginationInfo? Pagination { get; set; }

    [JsonPropertyName("data")]
    public List<AviationStackFlight>? Data { get; set; }
}

public class PaginationInfo
{
    [JsonPropertyName("limit")]
    public int? Limit { get; set; }

    [JsonPropertyName("offset")]
    public int? Offset { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }
}
