using BlogEngine.Shared.DTOs.Comment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogEngine.Api.Services.Abstractions
{
    public interface ICommentService
    {
        Task<CommentDTO> GetByIdAsync(int id);
        Task<List<CommentDTO>> GetByPostIdAsync(int id);
        Task<int> CreateAsync(CommentCreationDTO commentCreationDTO);
        Task UpdateAsync(int id, CommentUpdateDTO commentUpdateDTO);
        Task DeleteAsync(int id);
        Task<List<CommentDTO>> GetAllAsync();
    }
}