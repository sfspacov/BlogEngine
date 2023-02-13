using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BlogEngine.Core.Data.Entities;
using BlogEngine.Core.Services.Abstractions;
using BlogEngine.Shared.Helpers;
using BlogEngine.Shared.DTOs.Blog;
using BlogEngine.Api.Services.Abstractions;

namespace BlogEngine.Api.Services.Implementations
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IPostStatusRepository _postStatusRepository;
        private readonly IMapper _mapper;
        private readonly IReadingTimeEstimator _readingTimeEstimator;
        private readonly ICurrentUserProvider _currentUserProvider;

        public PostService(
            IPostRepository postRepository,
            IPostStatusRepository postStatusRepository,
            IMapper mapper,
            IReadingTimeEstimator readingTimeEstimator,
            ICurrentUserProvider currentUserProvider)
        {
            _postRepository = postRepository;
            _postStatusRepository = postStatusRepository;
            _mapper = mapper;
            _readingTimeEstimator = readingTimeEstimator;
            _currentUserProvider = currentUserProvider;
        }

        public async Task<PostDTO> GetByIdAsync(int id)
        {
            var postEntity = await _postRepository.GetByIdAsync(id);

            if (postEntity is null)
                throw new KeyNotFoundException($"There's no post with id={id}");

            var postDTO = ToDTO(postEntity);

            return postDTO;
        }

        public async Task<List<PostDTO>> GetAllByUserIdAsync(int id, string status = null)
        {
            var postEntities = await _postRepository.GetAllByUserIdAsync(id, status);

            return _mapper.Map<List<PostDTO>>(postEntities.ToList());
        }

        public async Task<List<PostDTO>> GetAllAsync(string status = null)
        {
            var postEntities = await _postRepository.GetAllAsync(status);

            return _mapper.Map<List<PostDTO>>(postEntities.ToList());
        }

        public async Task<int> CreateAsync(PostCreationDTO postCreationDTO)
        {
            Preconditions.NotNull(postCreationDTO, nameof(postCreationDTO));

            var postEntity = ToEntity(postCreationDTO);

            postEntity.EstimatedReadingTimeInMinutes = _readingTimeEstimator.GetEstimatedReadingTime(postEntity.Content);
            postEntity.PostStatus = await _postStatusRepository.GetByDescription(PostStatusEnum.PendingApproval.ToString());

            await AssignUserID(postEntity);

            return await _postRepository.CreateAsync(postEntity);
        }

        public async Task ApprovesAsync(int id, PostApprovesDTO postApprovesDTO)
        {
            var postEntity = await _postRepository.GetByIdAsync(id);

            if (postEntity is null)
                throw new KeyNotFoundException($"There's no post with id={id}");

            if (postEntity.PostStatusID != (int)PostStatusEnum.PendingApproval)
                throw new Exception("This post can't be approved/rejected because it's status is not 'pending aproval'");

            postEntity.PostStatus = await _postStatusRepository.GetByDescription(postApprovesDTO.Status.ToString());
            
            if (postApprovesDTO.Status.ToString().ToLower() == PostStatusEnum.Rejected.ToString().ToLower())
                postEntity.EditorsReview = postApprovesDTO.EditorsReview;
            else
                postEntity.EditorsReview = null;

            await _postRepository.UpdateAsync(postEntity);
        }

        public async Task UpdateAsync(int id, PostCreationDTO postUpdateDTO)
        {
            Preconditions.NotNull(postUpdateDTO, nameof(postUpdateDTO));

            var postEntity = await _postRepository.GetByIdAsync(id);

            if (postEntity is null)
                throw new ArgumentException();

            _mapper.Map(postUpdateDTO, postEntity);

            postEntity.EstimatedReadingTimeInMinutes = _readingTimeEstimator.GetEstimatedReadingTime(postUpdateDTO.Content);
            postEntity.PostStatus = await _postStatusRepository.GetByDescription(PostStatusEnum.PendingApproval.ToString());
            postEntity.EditorsReview = null;

            await _postRepository.UpdateAsync(postEntity);
        }

        public async Task DeleteAsync(int id)
        {
            var postEntity = await _postRepository.GetByIdAsync(id);

            if (postEntity is null)
                throw new ArgumentException();

            await _postRepository.DeleteAsync(postEntity.ID);
        }

        private async Task AssignUserID(Post post)
        {
            Preconditions.NotNull(post, nameof(post));

            var currentUser = await _currentUserProvider.GetCurrentUserAsync();

            if (currentUser is null)
            {
                throw new InvalidOperationException(nameof(AssignUserID));
            }
            else
            {
                post.ApplicationUserID = currentUser.Id;
            }
        }

        private Post ToEntity(PostCreationDTO postCreationDTO) => _mapper.Map<Post>(postCreationDTO);
        private PostDTO ToDTO(Post postEntity) => _mapper.Map<PostDTO>(postEntity);
        private PostCreationDTO ToUpdateDTO(Post postEntity) => _mapper.Map<PostCreationDTO>(postEntity);
    }
}