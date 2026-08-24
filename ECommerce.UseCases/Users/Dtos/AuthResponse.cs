namespace ECommerce.UseCases.Users.Dtos;

public record AuthResponse(
    string AccessToken,
    DateTimeOffset ExpiresAtUtc,
    string RefreshToken);