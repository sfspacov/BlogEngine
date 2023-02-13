using BlogEngine.Shared.DTOs.Identity;
using System.Threading.Tasks;

namespace BlogEngine.Api.Services.Abstractions.Identity
{
    public interface ITokenService
    {
        Task<UserTokenDTO> BuildToken(UserInfoDTO userInfoDTO);
    }
}