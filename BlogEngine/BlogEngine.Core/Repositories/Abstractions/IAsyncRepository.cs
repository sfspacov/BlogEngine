using System.Threading.Tasks;
using BlogEngine.Core.Data.Entities.Common;

namespace BlogEngine.Core.Services.Abstractions
{
    public interface IAsyncRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<int> CreateAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);
    }
}