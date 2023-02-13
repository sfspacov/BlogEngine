using System;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using BlogEngine.Core.Services.Abstractions;
using BlogEngine.Core.Data.Entities;
using BlogEngine.Shared.Helpers;
using BlogEngine.Shared.DTOs.Comment;
using BlogEngine.Api.Services.Abstractions.Identity;
using BlogEngine.Api.Services.Abstractions;
using BlogEngine.Shared.DTOs.Blog;

namespace BlogEngine.Api.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public CommentService(
            IPostRepository postRepository,
            ICommentRepository commentRepository,
            ICurrentUserProvider currentUserProvider,
            IAccountService accountService,
            IMapper mapper)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;
            _currentUserProvider = currentUserProvider;
            _accountService = accountService;
        }

        public async Task<List<CommentDTO>> GetAllAsync()
        {
            var commentsEntity = await _commentRepository.GetAllAsync();

            var commentsDTO = _mapper.Map<List<CommentDTO>>(commentsEntity);

            return commentsDTO;
        }

        public async Task<CommentDTO> GetByIdAsync(int id)
        {
            var commentEntity = await _commentRepository.GetByIdAsync(id);

            if (commentEntity == null) 
                throw new KeyNotFoundException($"There's no post with id={id}");

            var commentDTO = _mapper.Map<CommentDTO>(commentEntity);

            await BindUserInfoDetailDTOAsync(commentDTO);

            return commentDTO;
        }

        public async Task<List<CommentDTO>> GetByPostIdAsync(int id)
        {
            var postEntity = await _postRepository.GetByIdAsync(id);

            if (postEntity == null)
                throw new KeyNotFoundException($"There's no post with id={id}");

            var commentEntities = await _commentRepository.GetByPostIdAsync(id);

            var commentDTOs = _mapper.Map<List<CommentDTO>>(commentEntities.ToList());

            foreach (var commentDTO in commentDTOs)
            {
                await BindUserInfoDetailDTOAsync(commentDTO);
            }

            return commentDTOs;
        }

        public async Task<int> CreateAsync(CommentCreationDTO commentCreationDTO)
        {
            var postEntity = await _postRepository.GetByIdAsync(commentCreationDTO.PostID);

            if (postEntity == null)
                throw new KeyNotFoundException($"There's no post with id={commentCreationDTO.PostID}");

            if (postEntity.PostStatusID != (int)PostStatusEnum.Approved)
                throw new Exception("This post can't receive a commment because it's not published");

            Preconditions.NotNull(commentCreationDTO, nameof(commentCreationDTO));

            var commentEntity = _mapper.Map<Comment>(commentCreationDTO);

            commentEntity.ApplicationUserID = await _currentUserProvider.GetCurrentUserIDAsync();

            return await _commentRepository.CreateAsync(commentEntity);
        }

        public async Task DeleteAsync(int id)
        {
            var commentEntity = await _commentRepository.GetByIdAsync(id);

            if (commentEntity is null)
                throw new ArgumentException();

            await _commentRepository.DeleteAsync(id);
        }

        public async Task UpdateAsync(int id, CommentUpdateDTO commentUpdateDTO)
        {
            var commentEntity = await _commentRepository.GetByIdAsync(id);

            if (commentEntity is null)
                throw new ArgumentException();

            _mapper.Map(commentUpdateDTO, commentEntity);

            await _commentRepository.UpdateAsync(commentEntity);
        }

        private async Task BindUserInfoDetailDTOAsync(CommentDTO commentDTOBase)
        {
            commentDTOBase.UserInfoDetailDTO = await _accountService
                .GetUserInfoDetailDTOAsync(commentDTOBase.ApplicationUserID);
        }
    }
}