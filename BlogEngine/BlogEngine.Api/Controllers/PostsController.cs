using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using BlogEngine.Shared.DTOs;
using BlogEngine.Api.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using BlogEngine.Api.Common.Extensions;
using BlogEngine.Shared.DTOs.Blog;
using BlogEngine.Shared.Helpers;
using BlogEngine.Core.Services.Abstractions;
using BlogEngine.Api.Services.Abstractions.Identity;

namespace BlogEngine.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostsController : BaseController
    {
        private readonly IPostService _postService;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IRoleManager _roleManager;
        public PostsController(IPostService postService,
            IRoleManager roleManager,
            ICurrentUserProvider currentUserProvider)

        {
            _postService = postService;
            _roleManager = roleManager;
            _currentUserProvider = currentUserProvider;
        }

        [HttpGet("getById")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PostDTO>))]
        public async Task<ActionResult<PostDTO>> GetById(int id)
        {
            var currentUser = await _currentUserProvider.GetCurrentUserAsync();
            var post = await _postService.GetByIdAsync(id);

            //if the Post is published, anyone can see it
            if (post.Status == PostStatusEnum.Approved.ToString())
            {
                return post;
            }
            //if is not published, should be logged
            else if (currentUser is null)
            {
                return Unauthorized();
            }
            else if (post.Status == PostStatusEnum.Rejected.ToString())
            {
                var isTheAuthor = post.ApplicationUserID == currentUser.Id;
                if (isTheAuthor)
                {
                    return post;
                }
            }
            else if (post.Status == PostStatusEnum.PendingApproval.ToString())
            {
                var isTheAuthor = post.ApplicationUserID == currentUser.Id;
                var isAnEditor = await _roleManager.IsCurrentUserAnEditor(currentUser.Id);

                if (isTheAuthor || isAnEditor)
                {
                    return post;
                }
            }

            return Unauthorized();
        }

        [HttpGet("getAllPending")]
        [Authorize(Roles = UserRole.Editor)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PostDTO>))]
        public async Task<ActionResult<List<PostDTO>>> GetAllPending([FromQuery] PaginationDTO paginationDTO)
        {
            var postDTOs = await _postService.GetAllAsync(PostStatusEnum.PendingApproval.ToString());

            await HttpContext.InsertPaginationParametersInResponseAsync(postDTOs, paginationDTO.RecordsPerPage);

            return postDTOs.Paginate(paginationDTO).ToList();
        }

        [HttpGet("getRejected")]
        [Authorize(Roles = UserRole.Writer)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PostDTO>))]
        public async Task<ActionResult<List<PostDTO>>> GetRejected([FromQuery] PaginationDTO paginationDTO)
        {
            var currentUser = await _currentUserProvider.GetCurrentUserAsync();

            var postDTOs = await _postService.GetAllByUserIdAsync(currentUser.Id, PostStatusEnum.Rejected.ToString());

            await HttpContext.InsertPaginationParametersInResponseAsync(postDTOs, paginationDTO.RecordsPerPage);

            return postDTOs.Paginate(paginationDTO).ToList();
        }

        [HttpGet("getAllPublishedByUserId/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PostDTO>))]
        [AllowAnonymous]
        public async Task<ActionResult<List<PostDTO>>> GetAllPublishedByUserId(int id, [FromQuery] PaginationDTO paginationDTO)
        {
            var postDTOs = await _postService.GetAllByUserIdAsync(id, PostStatusEnum.Approved.ToString());

            await HttpContext.InsertPaginationParametersInResponseAsync(postDTOs, paginationDTO.RecordsPerPage);

            return postDTOs.Paginate(paginationDTO).ToList();
        }

        [HttpGet("getAllPublished")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PostDTO>))]
        public async Task<ActionResult<List<PostDTO>>> GetAllPublished([FromQuery] PaginationDTO paginationDTO)
        {
            var postDTOs = await _postService.GetAllAsync(PostStatusEnum.Approved.ToString());

            await HttpContext.InsertPaginationParametersInResponseAsync(postDTOs, paginationDTO.RecordsPerPage);

            return postDTOs.Paginate(paginationDTO).ToList();
        }

        [HttpGet("getOwnPosts")]
        [Authorize(Roles = UserRole.Writer)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PostDTO>))]
        public async Task<ActionResult<List<PostDTO>>> GetOwnPosts([FromQuery] PaginationDTO paginationDTO)
        {
            var currentUser = await _currentUserProvider.GetCurrentUserAsync();
            var postDTOs = await _postService.GetAllByUserIdAsync(currentUser.Id);

            await HttpContext.InsertPaginationParametersInResponseAsync(postDTOs, paginationDTO.RecordsPerPage);

            return postDTOs.Paginate(paginationDTO).ToList();
        }

        [HttpPost]
        [Authorize(Roles = UserRole.Writer)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> Post([FromBody] PostCreationDTO postCreationDTO)
        {
            if (postCreationDTO.Content is null || postCreationDTO.Title is null)
                return BadRequest();

            var id = await _postService.CreateAsync(postCreationDTO);

            return StatusCode(StatusCodes.Status201Created, new { PostId = id });
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = UserRole.Writer)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(PostDTO))]
        public async Task<ActionResult<PostDTO>> Put(int id, [FromBody] PostCreationDTO postUpdateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var post = await _postService.GetByIdAsync(id);

            if (post is null)
                return NotFound();

            if (post.Status != PostStatusEnum.Rejected.ToString())
                return BadRequest($"This post can't be updated because it's status is '{post.Status}'");

            var currentUser = await _currentUserProvider.GetCurrentUserAsync();

            if (currentUser.Id == post.ApplicationUserID)
            {
                await _postService.UpdateAsync(id, postUpdateDTO);

                return NoContent();
            }

            return BadRequest($"The user '{currentUser.FullName}' can not edit the post id={id}");
        }

        [HttpPut("approves/{id:int}")]
        [Authorize(Roles = UserRole.Editor)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Approves(int id, [FromBody] PostApprovesDTO postApprovesDTO)
        {
            await _postService.ApprovesAsync(id, postApprovesDTO);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public async Task<ActionResult> Delete(int id)
        {
            var currentUser = await _currentUserProvider.GetCurrentUserAsync();
            var isAdmin = (await _roleManager.IsCurrentUserAnAdmin(currentUser.Id));
            var post = await _postService.GetByIdAsync(id);

            if (post is null)
                return NotFound();

            if (currentUser.Id != post.ApplicationUserID && !isAdmin)
                return Unauthorized("You needs to be the author of the post or system admin");

            await _postService.DeleteAsync(id);

            return Ok();
        }
    }
}