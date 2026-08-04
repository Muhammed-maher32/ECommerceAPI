using Ardalis.Specification;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : BaseEntity
{
    Task<PagedResult<T>> PagedListAsync(ISpecification<T> specification, CancellationToken ct = default);
}