using System.Threading.Tasks;
using BlogEngine.Core.Data.Entities;

namespace BlogEngine.Core.Services.Abstractions
{
    public interface IPostStatusRepository : IAsyncRepository<PostStatus>
    {
        Task<PostStatus> GetByDescription(string description);
    }
}