using ECommerce.Domain.Shared;
using ECommerce.UseCases.Shared.Interfaces;
using ECommerce.UseCases.Users.Dtos;
using MediatR;

namespace ECommerce.UseCases.Users.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler(IIdentityService identityService) :
    IRequestHandler<GetCurrentUserQuery, Result<UserResponse>>
{
    public async Task<Result<UserResponse>> Handle(
        GetCurrentUserQuery request,
        CancellationToken cancellationToken)
    {
        var result = await identityService.GetUserByIdAsync(
            request.UserId, cancellationToken);

        return result.IsFailure
            ? Result<UserResponse>.Failure(result.Error!)
            : Result<UserResponse>.Success(new UserResponse(
                result.Value.UserId,
                result.Value.Email,
                result.Value.DisplayName));
    }
}
