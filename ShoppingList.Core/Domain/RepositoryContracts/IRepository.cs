using ShoppingList.Core.Domain.Entities;

namespace ShoppingList.Core.Domain.RepositoryContracts;

public interface IRepository<T> where T : IEntity
{
    Task<IReadOnlyCollection<T>> GetAllAsync();
    Task<T> GetByIdAsync(Guid id);
    Task<bool> ExistsAsync(string Name);
    Task InsertAsync(T entity);
    Task UpdateAsync(T entity);
    Task RemoveAsync(Guid id);
}
