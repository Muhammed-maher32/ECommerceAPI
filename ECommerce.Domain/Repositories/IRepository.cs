using Ardalis.Specification;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default);

    Task<T?> GetAsync(ISpecification<T> spec, CancellationToken ct = default);

    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, CancellationToken ct = default);

    Task<int> CountAsync(ISpecification<T> spec, CancellationToken ct = default);

    void Add(T entity);

    void Update(T entity);

    void Delete(T entity);
}
