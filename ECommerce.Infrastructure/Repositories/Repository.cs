using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories;

public sealed class Repository<T>(StoreDbContext dbContext)
    : IRepository<T> where T : BaseEntity
{
    private readonly DbSet<T> _dbSet = dbContext.Set<T>();
    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
         => await _dbSet.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default)
        => await _dbSet.ToListAsync(ct);

    public async Task<T?> GetAsync(ISpecification<T> spec, CancellationToken ct = default)
        => await ApplySpecification(spec).FirstOrDefaultAsync(ct);

    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, CancellationToken ct = default)
        => await ApplySpecification(spec).ToListAsync(ct);

    public async Task<int> CountAsync(ISpecification<T> spec, CancellationToken ct = default)
        => await ApplySpecification(spec).CountAsync(ct);

    public void Add(T entity) => _dbSet.Add(entity);

    public void Update(T entity) => _dbSet.Update(entity);

    public void Delete(T entity) => entity.MarkAsDeleted();

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        => SpecificationEvaluator.Default.GetQuery(_dbSet.AsQueryable(), spec);
}