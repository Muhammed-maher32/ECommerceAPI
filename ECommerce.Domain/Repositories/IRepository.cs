using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories;

public interface IRepository<T> : IReadRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default);

    //Why did not use ASYNC? i want just to register or 'write' i will add
    //i do add them in unitofwork.
    //Ardalis also have AddAsync but i wont use it, it breaks UoW
    void Add(T entity);

    void Update(T entity);

    void Delete(T entity);
}