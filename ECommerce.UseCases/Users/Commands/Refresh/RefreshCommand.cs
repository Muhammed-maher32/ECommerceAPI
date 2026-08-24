using ECommerce.Domain.Shared;
using ECommerce.UseCases.Users.Dtos;
using MediatR;

namespace ECommerce.UseCases.Users.Commands.Refresh;

public record RefreshCommand(string RefreshToken) : IRequest<Result<AuthResponse>>;