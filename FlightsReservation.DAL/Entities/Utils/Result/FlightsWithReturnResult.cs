using FlightsReservation.DAL.Entities.Model;

namespace FlightsReservation.DAL.Entities.Utils.Result;

public record FlightsWithReturnResult(IReadOnlyList<Flight> Outbound, IReadOnlyList<Flight> Return);