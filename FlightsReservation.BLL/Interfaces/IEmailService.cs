
namespace FlightsReservation.BLL.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, CancellationToken ct);
    bool Parse(string email);
}