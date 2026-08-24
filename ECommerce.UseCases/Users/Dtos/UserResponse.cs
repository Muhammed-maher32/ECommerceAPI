namespace ECommerce.UseCases.Users.Dtos;

public record UserResponse(
    Guid UserId,
    string Email,
    string? DisplayName);