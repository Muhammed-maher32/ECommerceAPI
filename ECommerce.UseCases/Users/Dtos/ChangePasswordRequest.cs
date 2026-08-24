namespace ECommerce.UseCases.Users.Dtos;

public record ChangePasswordRequest(
    string CurrentPassword,
    string NewPassword);