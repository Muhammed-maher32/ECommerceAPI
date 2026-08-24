namespace ECommerce.UseCases.Shared.Models;

public record AccessTokenResult(string AccessToken, DateTimeOffset ExpiresAtUtc);
