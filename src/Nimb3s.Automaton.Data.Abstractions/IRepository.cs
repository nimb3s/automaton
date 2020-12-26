using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nimb3s.Data.Abstractions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">The repository type</typeparam>
    /// <typeparam name="K">The data type of key the repository uses for its Id</typeparam>
    public interface IRepository<T> where T : IEntity<string>
    {
        Task<T> GetAsync(int Id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task DeleteAsync(T entity);
        Task UpdateAsync(T entity);
    }
}
