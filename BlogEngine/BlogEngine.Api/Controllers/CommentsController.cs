using BlogEngine.Core.Services.Abstractions;
using BlogEngine.Api.Services.Abstractions;
using BlogEngine.Api.Services.Abstractions.Identity;
using BlogEngine.Shared.DTOs.Comment;
using BlogEngine.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogEngine.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
    public class CommentsController : BaseController
    {
        private readonly ICommentService _commentService;
        private readonly IRoleManager _roleManager;
        private readonly ICurrentUserProvider _currentUserProvider;

        public CommentsController(ICommentService commentService,
            IRoleManager roleManager,
            ICurrentUserProvider currentUserProvider)
        {
            _currentUserProvider = currentUserProvider;
            _roleManager = roleManager;
            _commentService = commentService;
        }

        [HttpGet("getByPostId/{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CommentDTO>))]
        public async Task<ActionResult> GetByPostId(int id)
        {
            var result = await _commentService.GetByPostIdAsync(id);
            return Ok(result);
        }

        [HttpGet("getAll")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CommentDTO>))]
        public async Task<ActionResult> GetAll()
        {
            var result = await _commentService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("getById/{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentDTO))]
        public async Task<ActionResult> GetById(int id)
        {
            var commentDTO = await _commentService.GetByIdAsync(id);

            return Ok(commentDTO);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> Post(CommentCreationDTO commentCreationDTO)
        {
            var id = await _commentService.CreateAsync(commentCreationDTO);

            return StatusCode(StatusCodes.Status201Created, new { CommentId = id });
        }

        [HttpPut("{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public async Task<ActionResult> Put(int id, [FromBody] CommentUpdateDTO commentUpdateDTO)
        {
            var commentDTO = await _commentService.GetByIdAsync(id);

            if (commentDTO is null)
                return NotFound();

            var currentUser = await _currentUserProvider.GetCurrentUserAsync();
            var isAdmin = (await _roleManager.IsCurrentUserAnAdmin(currentUser.Id));

            if (currentUser.Id != commentDTO.ApplicationUserID && !isAdmin)
                return Unauthorized("You needs to be the author of the comment or system admin");

            await _commentService.UpdateAsync(id, commentUpdateDTO);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            var currentUser = await _currentUserProvider.GetCurrentUserAsync();
            var isAdmin = (await _roleManager.IsCurrentUserAnAdmin(currentUser.Id));
            var commentDTO = await _commentService.GetByIdAsync(id);

            if (commentDTO is null)
                return NotFound();

            if (currentUser.Id != commentDTO.ApplicationUserID && !isAdmin)
                return Unauthorized("You needs to be the author of the comment or system admin");

            await _commentService.DeleteAsync(id);
            return Ok();
        }
    }
}