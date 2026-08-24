using ECommerce.Domain.Shared;
using ECommerce.UseCases.Shared.Interfaces;
using ECommerce.UseCases.Users.Dtos;
using MediatR;

namespace ECommerce.UseCases.Users.Commands.Login;

public class LoginCommandHandler(
    IIdentityService identityService,
    IJwtTokenGenerator jwtTokenGenerator) :
    IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var validateResult = await identityService.ValidateCredentialsAsync(
            request.Email,
            request.Password,
            cancellationToken);

        if (validateResult.IsFailure)
            return Result<AuthResponse>.Failure(validateResult.Error!);

        return await AuthResponseBuilder.BuildAsync(
            identityService, jwtTokenGenerator, validateResult.Value, cancellationToken);
    }
}
