using ECommerce.Domain.Shared;
using MediatR;

namespace ECommerce.UseCases.Users.Commands.ForgotPassword;

public record ForgotPasswordCommand(string Email) : IRequest<Result>;
