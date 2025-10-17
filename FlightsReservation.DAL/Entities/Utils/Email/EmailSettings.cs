namespace FlightsReservation.DAL.Entities.Utils.Email;

public sealed class EmailSettings
{
    public string ImapServer { get; set; } = null!;
    public int ImapPort { get; set; }
    public string SmtpServer { get; set; } = null!;
    public int SmtpPort { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ApplicationName { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string HtmlBody { get; set; } = null!;
}