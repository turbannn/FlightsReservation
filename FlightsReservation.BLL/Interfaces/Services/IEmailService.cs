namespace FlightsReservation.BLL.Interfaces.Services;

public interface IEmailService
{
    Task SendEmailAsync(string to, MemoryStream pdfStream, string attachmentName, CancellationToken ct);
    bool Parse(string email);
}