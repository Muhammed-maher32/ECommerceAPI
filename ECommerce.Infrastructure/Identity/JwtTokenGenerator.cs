using ECommerce.UseCases.Common;
using ECommerce.UseCases.Common.Interfaces;
using ECommerce.UseCases.Common.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerce.Infrastructure.Identity;

public sealed class JwtTokenGenerator(IOptions<JwtSettings> settings) :
    IJwtTokenGenerator
{
    private readonly JwtSettings _settings = settings.Value;

    public AccessTokenResult GenerateToken(Guid userId, string email, string? displayName, IEnumerable<string> roles)
    {
        //the time token genearted ,, add the expiration time to it.
        var expiresAt = DateTimeOffset.UtcNow
            .AddMinutes(_settings.AccessTokenExpirationMinutes);

        var claims = new List<Claim>
        {
            new Claim (JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),

            new Claim (JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Email, email),

            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

            // No 'exp' claim here: JwtSecurityToken writes it from the 'expires'
            // argument below, as a NumericDate. Adding it by hand would emit a second,
            // non-numeric exp and lifetime validation would reject the token.
        };

        if (!string.IsNullOrWhiteSpace(displayName))
        {
            claims.Add(new Claim("display_name", displayName)); //key -> value (Custom Claim)
        }

        claims.AddRange(roles.Select
            (
            role => new Claim(ClaimTypes.Role, role)
            ));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret)); //Hashing Key

        //Hashing Alogrithm
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials
            );

        var written = new JwtSecurityTokenHandler().WriteToken(token);

        return new AccessTokenResult(written, expiresAt);
    }
}
