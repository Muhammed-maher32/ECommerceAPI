using ECommerce.Domain.Shared;
using ECommerce.UseCases.Shared.Interfaces;
using ECommerce.UseCases.Users.Dtos;
using MediatR;

namespace ECommerce.UseCases.Users.Commands.Register;

public class RegisterCommandHandler(
    IIdentityService identityService,
    IJwtTokenGenerator jwtTokenGenerator) :
    IRequestHandler<RegisterCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var createResult = await identityService.CreateUserAsync(
           request.Email,
           request.Password,
           request.DisplayName,
           cancellationToken
        );

        if (createResult.IsFailure)
            return Result<AuthResponse>.Failure(createResult.Error!);

        return await AuthResponseBuilder.BuildAsync(
            identityService, jwtTokenGenerator, createResult.Value, cancellationToken);
    }
}
