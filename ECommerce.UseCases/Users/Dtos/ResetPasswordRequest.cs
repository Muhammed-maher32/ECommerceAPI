namespace ECommerce.UseCases.Users.Dtos;

public record ResetPasswordRequest(
    string Email,
    string Token,
    string NewPassword);