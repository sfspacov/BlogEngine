using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlogEngine.Core.Data.DatabaseContexts;
using BlogEngine.Core.Data.Entities;
using BlogEngine.Core.Services.Abstractions;
using BlogEngine.Shared.DTOs.Blog;
using System;

namespace BlogEngine.Core.Services.Implementations
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<IEnumerable<Post>> GetAllAsync(string status = null)
        {
            var query = _context.Posts
                .Include(p => p.PostStatus)
                .AsQueryable();

            if (status != null)
            {
                query = query.Where(x => x.PostStatus.Description == status);
            }

            return Task.FromResult(query
                 .Include(b => b.Comments)
                 .OrderBy(b => b.DateCreated)
                 .ThenByDescending(b => b.LastUpdateDate)
                 .AsEnumerable());
        }
        public override async Task<Post> GetByIdAsync(int id)
        {
            return await _context.Posts
                .Include(p => p.PostStatus)
                .Where(b => b.ID.Equals(id))
                .Include(b => b.Comments)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Post>> GetAllByUserIdAsync(int id, string status = null)
        {
            var query = _context.Posts
                .Include(p => p.PostStatus)
                .Where(p => p.ApplicationUserID == id)
                ;

            if (status != null)
            {
                PostStatusEnum myStatus;
                Enum.TryParse(status, out myStatus);
                var statusId = (int)myStatus;
                query = query.Where(p => p.PostStatusID == statusId);
            }

            var posts = query
                .OrderBy(b => b.DateCreated)
                .ThenByDescending(b => b.LastUpdateDate);

            return posts;
        }

        public override async Task<int> CreateAsync(Post entity)
        {
            var id = await base.CreateAsync(entity);

            _context.Entry(entity);

            return id;
        }

        public override async Task UpdateAsync(Post entity)
        {
            await base.UpdateAsync(entity);

            _context.Entry(entity);
        }
    }
}