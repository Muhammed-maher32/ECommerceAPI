using ECommerce.Domain.Shared;
using MediatR;

namespace ECommerce.UseCases.Users.Commands.ResetPassword;

public record ResetPasswordCommand(
    string Email,
    string Token,
    string NewPassword) : IRequest<Result>;
