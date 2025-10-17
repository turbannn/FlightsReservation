
namespace FlightsReservation.BLL.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, MemoryStream pdfStream, string attachmentName, CancellationToken ct);
    bool Parse(string email);
}