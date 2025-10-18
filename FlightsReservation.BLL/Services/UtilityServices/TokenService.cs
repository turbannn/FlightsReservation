using System.Security.Claims;
using System.Text;
using FlightsReservation.DAL.Entities.Utils.Authentication;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace FlightsReservation.BLL.Services.UtilityServices;

public class TokenService
{
    private readonly JwtSettings _jwtSettings;

    public TokenService(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }

    public string CreateAccessToken(Guid id, string role)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

        var securityCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var descr = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
                new Claim(ClaimTypes.Role, role)
            ]),
            Expires = DateTime.Now.AddMinutes(_jwtSettings.ExpiryMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = securityCred
        };

        var handler = new JsonWebTokenHandler();

        return handler.CreateToken(descr);
    }
}