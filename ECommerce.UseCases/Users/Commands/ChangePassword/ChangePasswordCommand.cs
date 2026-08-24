using ECommerce.Domain.Shared;
using MediatR;

namespace ECommerce.UseCases.Users.Commands.ChangePassword;

public record ChangePasswordCommand(
Guid UserId,
string CurrentPassword,
string NewPassword) : IRequest<Result>;
