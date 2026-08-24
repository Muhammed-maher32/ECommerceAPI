namespace ECommerce.UseCases.Users.Dtos;

public record RegisterRequest(
    string Email,
    string Password,
    string? DisplayName);
