using BlogEngine.Shared.DTOs;
using NUnit.Framework;
using Moq;
using BlogEngine.Core.Services.Abstractions;
using BlogEngine.Api.Services.Implementations;
using AutoMapper;
using System.Threading.Tasks;
using BlogEngine.Core.Data.Entities;
using BlogEngine.Core.Services.Implementations;
using BlogEngine.Shared.DTOs.Blog;
using BlogEngine.Api.Common.Mappings;
using System.Collections.Generic;
using System;
using BlogEngine.Api.Controllers;
using BlogEngine.Api.Services.Abstractions.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace BlogEngine.Tests.BlogEngine.Api.Controllers
{
    [TestFixture]
    public class PostsControllerTests
    {
        private Mock<IPostRepository> _postRepository;
        private Mock<IPostStatusRepository> _postStatusRepository;
        private Mock<IRoleManager> _roleManager;
        private Mock<ICurrentUserProvider> _currentUserProvider;
        private IReadingTimeEstimator _readingTimeEstimator;
        private IMapper _mapper;
        private PostService _postService;
        private PostsController _postsController;
        [SetUp]
        public void SetUp()
        {

            var myProfile = new AutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            _postRepository = new Mock<IPostRepository>();
            _postStatusRepository = new Mock<IPostStatusRepository>();
            _currentUserProvider = new Mock<ICurrentUserProvider>();
            _roleManager = new Mock<IRoleManager>();
            _readingTimeEstimator = new ReadingTimeEstimator();
            _mapper = new Mapper(configuration);
            _postService = new PostService(_postRepository.Object, _postStatusRepository.Object, _mapper, _readingTimeEstimator, _currentUserProvider.Object);
            var httpContext = new DefaultHttpContext();

            _postsController = new PostsController(_postService, _roleManager.Object, _currentUserProvider.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
        }

        [Test]

        public async Task Post_Ok()
        {
            //Arrange
            _postStatusRepository
                .Setup(t => t.GetByDescription(It.IsAny<string>()))
                .ReturnsAsync(new PostStatus());

            _postRepository
                .Setup(t => t.CreateAsync(It.IsAny<Post>()))
                .ReturnsAsync(1);

            _currentUserProvider
                .Setup(t => t.GetCurrentUserAsync())
                .ReturnsAsync(new ApplicationUser { Id = 1 });

            //Act
            var actionResult = await _postsController.Post(
                new PostCreationDTO
                {
                    Content = "Be or not be, that's the question",
                    Title = "Hamlet"
                });

            //Assert
            var result = actionResult as ObjectResult;
            Assert.NotNull(result);
            Assert.AreEqual(result.StatusCode, StatusCodes.Status201Created);
        }

        [Test]

        public async Task Post_NullValues_BadRequest()
        {
            //Arrange
            _postStatusRepository
                .Setup(t => t.GetByDescription(It.IsAny<string>()))
                .ReturnsAsync(new PostStatus());

            _postRepository
                .Setup(t => t.CreateAsync(It.IsAny<Post>()))
                .ReturnsAsync(1);

            _currentUserProvider
                .Setup(t => t.GetCurrentUserAsync())
                .ReturnsAsync(new ApplicationUser { Id = 1 });

            //Act
            var actionResult = await _postsController.Post(
                new PostCreationDTO
                {
                    Content = null,
                    Title = null
                });

            //Assert
            var result = actionResult as BadRequestResult;
            Assert.NotNull(result);
            Assert.AreEqual(result.StatusCode, StatusCodes.Status400BadRequest);
        }

        [Test]
        public async Task GetAllPending_Ok()
        {
            //Arrange
            _postRepository
                .Setup(t => t.GetAllAsync(PostStatusEnum.PendingApproval.ToString()))
                .ReturnsAsync(new List<Post>() { new Post { } });

            //Act
            var actionResult = await _postsController.GetAllPending(new PaginationDTO());

            //Assert
            var result = actionResult.Value;
            Assert.NotNull(result);
            Assert.NotZero(result.Count);
        }

        [Test]
        public async Task GetAllPublished_Ok()
        {
            //Arrange
            _postRepository
                .Setup(t => t.GetAllAsync(PostStatusEnum.Approved.ToString()))
                .ReturnsAsync(new List<Post>() { new Post { } });

            //Act
            var actionResult = await _postsController.GetAllPublished(new PaginationDTO());

            //Assert
            var result = actionResult.Value;
            Assert.NotNull(result);
            Assert.NotZero(result.Count);
        }

        [Test]
        [TestCase(1)]
        public async Task GetAllPublishedByUserId_Ok(int id)
        {
            //Arrange
            _postRepository
                .Setup(t => t.GetAllByUserIdAsync(id, PostStatusEnum.Approved.ToString()))
                .ReturnsAsync(new List<Post>() { new Post { } });

            //Act
            var actionResult = await _postsController.GetAllPublishedByUserId(id, new PaginationDTO());

            //Assert
            var result = actionResult.Value;
            Assert.NotNull(result);
            Assert.NotZero(result.Count);
        }


        [Test]
        [TestCase(0)]
        public async Task GetAllPublishedByUserId_InvalidId_EmptyList(int id)
        {
            //Arrange
            _postRepository
                .Setup(t => t.GetAllByUserIdAsync(id, PostStatusEnum.Approved.ToString()))
                .ReturnsAsync(new List<Post>() { });

            //Act
            var actionResult = await _postsController.GetAllPublishedByUserId(id, new PaginationDTO());

            //Assert
            var result = actionResult.Value;
            Assert.NotNull(result);
            Assert.Zero(result.Count);
        }

        [Test]
        [TestCase(1)]
        public async Task GetById_Ok(int id)
        {
            //Arrange
            _postRepository
                .Setup(t => t.GetByIdAsync(id))
                .ReturnsAsync(new Post { });

            //Act
            var actual = await _postsController.GetById(id);

            //Assert
            Assert.IsNotNull(actual);
        }

        [Test]
        [TestCase(-1)]
        public void GetById_NegativeId_ThrowsKeyNotFoundException(int id)
        {
            //Arrange
            _postRepository
                .Setup(t => t.GetByIdAsync(id))
                .ReturnsAsync(null, new TimeSpan(5));

            //Act
            //Assert
            Assert.ThrowsAsync<KeyNotFoundException>(() => _postsController.GetById(id));
        }

        [Test]
        [TestCase(1)]
        public async Task GetRejected_Ok(int id)
        {
            //Arrange
            _currentUserProvider
                .Setup(t => t.GetCurrentUserAsync())
                .ReturnsAsync(new ApplicationUser { Id = 1 });

            //Act
            var actual = await _postsController.GetRejected(new PaginationDTO());

            //Assert
            Assert.IsNotNull(actual);
        }

        [Test]
        [TestCase(1)]
        public async Task GetOwnPosts_Ok(int id)
        {
            //Arrange
            _currentUserProvider
                .Setup(t => t.GetCurrentUserAsync())
                .ReturnsAsync(new ApplicationUser { Id = 1 });

            //Act
            var actual = await _postsController.GetOwnPosts(new PaginationDTO());

            //Assert
            Assert.IsNotNull(actual);
        }

        [Test]
        public async Task Put_Ok()
        {
            //Arrange
            _postRepository
                .Setup(t => t.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Post { ID = 1, Title = "Pinocchio", Content = "Once upon a time..." });

            _postStatusRepository
                .Setup(t => t.GetByDescription(It.IsAny<string>()))
                .ReturnsAsync(new PostStatus());

            //Act
            await _postsController.Put(1, new PostCreationDTO { Content = "Bla, bla, bla", Title = "Small talk" });
        }


       [Test]
        public async Task Delete_Ok()
        {
            //Arrange
            _postRepository
                .Setup(t => t.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Post { ID = 1, Title = "Pinocchio", Content = "Once upon a time..." });

            _currentUserProvider
                .Setup(t => t.GetCurrentUserAsync())
                .ReturnsAsync(new ApplicationUser { Id = 1 });

            //Act
            await _postsController.Delete(1);
        }
    }
}