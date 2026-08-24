using ECommerce.Domain.Shared;
using ECommerce.UseCases.Users.Dtos;
using MediatR;

namespace ECommerce.UseCases.Users.Commands.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string? DisplayName) : IRequest<Result<AuthResponse>>;
