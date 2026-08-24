using ECommerce.Domain.Shared;
using ECommerce.UseCases.Users.Dtos;
using MediatR;

namespace ECommerce.UseCases.Users.Commands.Login;

public record LoginCommand(
    string Email,
    string Password) : IRequest<Result<AuthResponse>>;