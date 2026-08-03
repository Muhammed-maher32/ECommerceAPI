using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories;

public class Repository<T>(StoreDbContext context)
    : IRepository<T> where T : BaseEntity
{
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _dbSet.FindAsync([id], ct);
    }

    public async Task<T?> GetAsync(
        ISpecification<T> specification,
        CancellationToken ct = default)
    {
        return await ApplySpecification(specification)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<TResult?> GetAsync<TResult>(
        ISpecification<T, TResult> specification,
        CancellationToken ct = default)
    {
        return await ApplySpecification(specification)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<T>> ListAsync(
        ISpecification<T> specification,
        CancellationToken ct = default)
    {
        return await ApplySpecification(specification)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<TResult>> ListAsync<TResult>(
        ISpecification<T, TResult> specification,
        CancellationToken ct = default)
    {
        return await ApplySpecification(specification)
            .ToListAsync(ct);
    }

    public async Task<int> CountAsync(
        ISpecification<T> specification,
        CancellationToken ct = default)
    {
        return await ApplySpecification(specification)
            .CountAsync(ct);
    }

    public async Task AddAsync(T entity, CancellationToken ct = default)
    {
        await _dbSet.AddAsync(entity, ct);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    private IQueryable<T> ApplySpecification(
        ISpecification<T> specification)
    {
        return SpecificationEvaluator.Default.GetQuery(
            _dbSet.AsQueryable(),
            specification);
    }

    private IQueryable<TResult> ApplySpecification<TResult>(
        ISpecification<T, TResult> specification)
    {
        return SpecificationEvaluator.Default.GetQuery(
            _dbSet.AsQueryable(),
            specification);
    }
}