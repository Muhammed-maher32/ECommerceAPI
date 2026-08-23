using Ardalis.Specification;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories;

//Inherts IReadRepositoryBase from Adalis.Specification
//It hase : FirstOrDefault,Count,List ASYNC -> takes Specification
public interface IReadRepository<T> : IReadRepositoryBase<T> where T : BaseEntity
{
    Task<PagedResult<T>> PagedListAsync(
        ISpecification<T> specification,
        CancellationToken ct = default
        );
}