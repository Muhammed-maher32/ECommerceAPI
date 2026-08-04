using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories;

public class Repository<T>(StoreDbContext context)
    : RepositoryBase<T>(context), IRepository<T> where T : BaseEntity
{
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _dbSet.FindAsync([id], ct);

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default)
        => await _dbSet.AsNoTracking().ToListAsync(ct);

    public async Task<PagedResult<T>> PagedListAsync(ISpecification<T> specification, CancellationToken ct = default)
    {
        var totalCount = await CountAsync(specification, ct);
        var items = await ListAsync(specification, ct);

        return new PagedResult<T>(items, totalCount);
    }

    public void Add(T entity) => _dbSet.Add(entity);

    public void Update(T entity) => _dbSet.Update(entity);

    public void Delete(T entity)
        => entity.MarkAsDeleted();
}