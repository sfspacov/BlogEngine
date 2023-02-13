using BlogEngine.Shared.DTOs.Blog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogEngine.Api.Services.Abstractions
{
    public interface IPostService
    {
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, PostCreationDTO postUpdateDTO);
        Task<PostDTO> GetByIdAsync(int id);
        Task<int> CreateAsync(PostCreationDTO postCreationDTO);
        Task<List<PostDTO>> GetAllByUserIdAsync(int id, string status = null);
        Task<List<PostDTO>> GetAllAsync(string status);
        Task ApprovesAsync(int id, PostApprovesDTO postApprovesDTO);
    }
}