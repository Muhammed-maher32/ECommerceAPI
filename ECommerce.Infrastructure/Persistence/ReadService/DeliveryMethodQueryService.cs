using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.UseCases.DeliveryMethods;
using ECommerce.UseCases.DeliveryMethods.Dtos;
using ECommerce.UseCases.Products.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.ReadService;

public sealed class DeliveryMethodQueryService(StoreDbContext
    storeDbContext) : IDeliveryMethodQueryService
{
    public async Task<IReadOnlyList<GetAllDeliveryMethodsResponse>> GetAllDeliveryMethodsAsync(CancellationToken ct = default)
    {
        return await storeDbContext.DeliveryMethods
               .AsNoTracking()
               .OrderBy(x => x.Price) //Maybe More
               .ProjectToType<GetAllDeliveryMethodsResponse>()
               .ToListAsync(ct);
    }
}
