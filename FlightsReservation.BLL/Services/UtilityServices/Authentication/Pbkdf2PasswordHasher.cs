using FlightsReservation.BLL.Interfaces.Services;
using System.Security.Cryptography;

namespace FlightsReservation.BLL.Services.UtilityServices.Authentication;

public class Pbkdf2PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 150, HashAlgorithmName.SHA256, 32);

        var saltBase64 = Convert.ToBase64String(salt);
        var hashBase64 = Convert.ToBase64String(hash);

        return $"{saltBase64}:{hashBase64}";
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        var parts = hashedPassword.Split(':');
        if (parts.Length != 2) return false;

        var salt = Convert.FromBase64String(parts[0]);
        var hash = Convert.FromBase64String(parts[1]);

        var newHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 150, HashAlgorithmName.SHA256, 32);

        return CryptographicOperations.FixedTimeEquals(hash, newHash);
    }
}
