using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlogEngine.Core.Data.DatabaseContexts;
using BlogEngine.Core.Data.Entities;
using BlogEngine.Core.Services.Abstractions;

namespace BlogEngine.Core.Services.Implementations
{
    public class PostStatusRepository : Repository<PostStatus>, IPostStatusRepository
    {
        private readonly ApplicationDbContext _context;

        public PostStatusRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PostStatus> GetByDescription(string description)
        {
            var result = await _context.PostStatus.FirstOrDefaultAsync(x => x.Description == description);
            return result;
        }
    }
}