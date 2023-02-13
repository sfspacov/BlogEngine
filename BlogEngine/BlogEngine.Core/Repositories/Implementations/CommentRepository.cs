using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using BlogEngine.Core.Data.Entities;
using BlogEngine.Core.Services.Abstractions;
using BlogEngine.Core.Data.DatabaseContexts;
using Microsoft.EntityFrameworkCore;
using BlogEngine.Shared.Helpers;
using BlogEngine.Core.Common.Exceptions;

namespace BlogEngine.Core.Services.Implementations
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual async Task<Comment> GetByIdAsync(int id)
        {
            return await _context.Comments
                .FirstOrDefaultAsync(mc => mc.ID.Equals(id));
        }

        public virtual Task<IEnumerable<Comment>> GetByPostIdAsync(int id)
        {
            return Task.FromResult(_context.Comments
                .Where(mc => mc.PostID.Equals(id))
                .AsEnumerable());
        }

        public virtual async Task<int> CreateAsync(Comment comment)
        {
            Preconditions.NotNull(comment, nameof(comment));

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return comment.ID;
        }

        public virtual async Task UpdateAsync(Comment comment)
        {
            Preconditions.NotNull(comment, nameof(comment));

            _context.Comments.Update(comment);

            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entityFromDb = await GetByIdAsync(id);

            if (entityFromDb is null)
            {
                throw new EntityNotFoundException(nameof(Comment));
            }

            _context.Comments.Remove(entityFromDb);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }
    }
}