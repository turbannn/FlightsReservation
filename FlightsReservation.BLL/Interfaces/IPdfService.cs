using FlightsReservation.BLL.Entities.DataTransferObjects.FlightDtos;
using FlightsReservation.DAL.Entities.Model;

namespace FlightsReservation.BLL.Interfaces;

public interface IPdfService
{
    Task<MemoryStream> GenerateTicketPdfAsync(Passenger passenger, Flight flight);
}