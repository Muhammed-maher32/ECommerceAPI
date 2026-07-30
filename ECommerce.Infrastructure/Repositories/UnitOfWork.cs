using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Persistence.DbContexts;
using System.Collections.Concurrent;

namespace ECommerce.Infrastructure.Repositories;

//why unit of work??? you tell me what repo u want, and i return it to you
public class UnitOfWork(StoreDbContext dbContext) : IUnitOfWork
{
    //type(product)=>Irepo<product>

    private readonly ConcurrentDictionary<Type, object> _repos = new();
    public IRepository<T> Repository<T>() where T : BaseEntity
    {
        var type = typeof(T);
        if (_repos.TryGetValue(type, out var repo))
            return (IRepository<T>)repo;

        var newRepo = new Repository<T>(dbContext);

        _repos.TryAdd(type, newRepo);

        return newRepo;
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
