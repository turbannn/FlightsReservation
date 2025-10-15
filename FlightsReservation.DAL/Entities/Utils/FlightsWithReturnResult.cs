using FlightsReservation.DAL.Entities.Model;

namespace FlightsReservation.DAL.Entities.Utils;

public record FlightsWithReturnResult(IReadOnlyList<Flight> Outbound, IReadOnlyList<Flight> Return);