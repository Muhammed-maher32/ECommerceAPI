using ECommerce.Domain.Shared;
using MediatR;

namespace ECommerce.UseCases.Users.Commands.Logout;

public record LogoutCommand(string RefreshToken) : IRequest<Result>;
