using Ardalis.Specification;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<T?> GetAsync(
        ISpecification<T> specification,
        CancellationToken ct = default);

    Task<TResult?> GetAsync<TResult>(
        ISpecification<T, TResult> specification,
        CancellationToken ct = default);

    Task<IReadOnlyList<T>> ListAsync(
        ISpecification<T> specification,
        CancellationToken ct = default);

    Task<IReadOnlyList<TResult>> ListAsync<TResult>(
        ISpecification<T, TResult> specification,
        CancellationToken ct = default);

    Task<int> CountAsync(
        ISpecification<T> specification,
        CancellationToken ct = default);

    Task AddAsync(T entity, CancellationToken ct = default);

    void Update(T entity);

    void Delete(T entity);
}
