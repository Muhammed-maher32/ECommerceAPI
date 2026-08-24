namespace ECommerce.UseCases.Shared.Models;

public sealed record AuthUserSnapshot(Guid UserId, string Email, string? DisplayName);

