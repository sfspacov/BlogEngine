using System.Collections.Generic;
using System.Threading.Tasks;
using BlogEngine.Core.Data.Entities;

namespace BlogEngine.Core.Services.Abstractions
{
    public interface IPostRepository : IAsyncRepository<Post>
    {
        Task<IEnumerable<Post>> GetAllAsync(string status = null);
        Task<IEnumerable<Post>> GetAllByUserIdAsync(int id, string status = null);
    }
}