using ECommerce.UseCases.Shared.Models;

namespace ECommerce.UseCases.Shared.Interfaces;


public interface IJwtTokenGenerator
{
    AccessTokenResult GenerateToken(Guid userId,
        string email,
        string? displayName,
        IEnumerable<String> roles);
}
