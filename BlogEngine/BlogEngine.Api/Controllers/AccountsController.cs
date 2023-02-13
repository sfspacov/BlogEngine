using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Collections.Generic;
using BlogEngine.Shared.Helpers;
using BlogEngine.Shared.DTOs.Identity;
using BlogEngine.Api.Services.Abstractions.Identity;
using BlogEngine.Core.Services.Abstractions;
using System.Linq;
using System.Security.Claims;

namespace BlogEngine.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountsController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IRoleManager _roleManager;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly ITokenService _tokenService;

        public AccountsController(
            IAccountService accountService,
            IAuthenticationService authenticationService,
            IRoleManager roleManager,
            ICurrentUserProvider currentUserProvider,
            ITokenService tokenService)
        {
            _accountService = accountService;
            _authenticationService = authenticationService;
            _roleManager = roleManager;
            _currentUserProvider = currentUserProvider;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UserTokenDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserTokenDTO>> Register([FromBody] UserRegisterDTO userRegisterDTO)
        {
            var identityResult = await _authenticationService.RegisterAsync(userRegisterDTO);

            if (!identityResult.Succeeded) return BadRequest(identityResult.Errors);

            return await _tokenService.BuildToken(userRegisterDTO);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UserTokenDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserTokenDTO>> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            var signInResult = await _authenticationService.LoginAsync(userLoginDTO);

            if (!signInResult.Succeeded) return BadRequest("Invalid Login attempt");

            return await _tokenService.BuildToken(userLoginDTO);
        }

        [HttpGet("users")]
        [Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(typeof(List<UserInfoDetailDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<UserInfoDetailDTO>>> GetUsers()
        {
            return await _accountService.GetUserInfoDetailDTOsAsync();
        }

        [HttpGet("userById/{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UserProfileDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserProfileDTO>> GetUserById(int id)
        {
            var currentUser = await _currentUserProvider.GetCurrentUserAsync();
            var isAdmin = (await _roleManager.IsCurrentUserAnAdmin(currentUser.Id));

            if (currentUser.Id != id && !isAdmin)
                return Unauthorized("You needs to be the user that you're trying to get information or system admin");

            var userProfileDTO = await _accountService.GetUserProfileDTOAsync(id);

            if (userProfileDTO is null) return NotFound();

            return userProfileDTO;
        }

        [HttpPut("update/{email}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateUser(string email, [FromBody] UserUpdateDTO userUpdateDTO)
        {
            var currentUser = await _currentUserProvider.GetCurrentUserAsync();
            var isAdmin = (await _roleManager.IsCurrentUserAnAdmin(currentUser.Id));

            if (currentUser.Email != email && !isAdmin)
                return Unauthorized("You needs to be the user that you're trying to get information or system admin");

            await _accountService.UpdateUserAsync(email, userUpdateDTO);
            return NoContent();
        }

        [HttpPost("assignRole")]
        [Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> AssignRole([FromBody] UserRoleDTO userRoleDTO)
        {
            var assignmentResult = await _roleManager.AssignRoleAsync(userRoleDTO);

            if (assignmentResult.UserNotFound) return NotFound();

            return assignmentResult.Successed;
        }

        [HttpDelete("removeRole")]
        [Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> RemoveRole([FromBody] UserRoleDTO userRoleDTO)
        {
            var assignationResult = await _roleManager.RemoveRoleAsync(userRoleDTO);

            if (assignationResult.UserNotFound) return NotFound();

            return assignationResult.Successed;
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteUser(int id)
        {
            var currentUser = await _currentUserProvider.GetCurrentUserAsync();
            var isAdmin = (await _roleManager.IsCurrentUserAnAdmin(currentUser.Id));

            if (currentUser.Id != id && !isAdmin)
                return Unauthorized("You needs to be the user that you're trying to delete or system admin");

            var deleteResult = await _accountService.DeleteAsync(id);

            if (deleteResult.UserNotFound) return NotFound();

            return deleteResult.Successed;
        }

        [HttpPost("renewToken")]
        public async Task<ActionResult<UserTokenDTO>> RenewToken()
        {
            var user = HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var emailClaim = user.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email));

            if (emailClaim is null)
            {
                return Unauthorized();
            }

            var userInfo = new UserInfoDTO()
            {
                EmailAddress = emailClaim.Value
            };

            return await _tokenService.BuildToken(userInfo);
        }
    }
}