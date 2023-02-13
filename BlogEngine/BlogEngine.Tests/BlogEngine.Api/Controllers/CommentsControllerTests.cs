using NUnit.Framework;
using Moq;
using BlogEngine.Core.Services.Abstractions;
using BlogEngine.Api.Services.Implementations;
using AutoMapper;
using System.Threading.Tasks;
using BlogEngine.Core.Data.Entities;
using BlogEngine.Shared.DTOs.Blog;
using BlogEngine.Api.Common.Mappings;
using System.Collections.Generic;
using System;
using BlogEngine.Api.Controllers;
using BlogEngine.Api.Services.Abstractions.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BlogEngine.Shared.DTOs.Comment;

namespace BlogEngine.Tests.BlogEngine.Api.Controllers
{
    [TestFixture]
    public class CommentsControllerTests
    {
        private Mock<IPostRepository> _postRepository;
        private Mock<IAccountService> _accountService;
        private Mock<IRoleManager> _roleManager;
        private Mock<ICommentRepository> _commentRepository;
        private Mock<ICurrentUserProvider> _currentUserProvider;
        private IMapper _mapper;
        private CommentService _commentService;
        private CommentsController _commentController;
        [SetUp]
        public void SetUp()
        {

            var myProfile = new AutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            _postRepository = new Mock<IPostRepository>();
            _roleManager = new Mock<IRoleManager>();
            _accountService = new Mock<IAccountService>();
            _commentRepository = new Mock<ICommentRepository>();
            _currentUserProvider = new Mock<ICurrentUserProvider>();
            _mapper = new Mapper(configuration);
            _commentService = new CommentService(_postRepository.Object, _commentRepository.Object, _currentUserProvider.Object, _accountService.Object, _mapper);
            var httpContext = new DefaultHttpContext();

            _commentController = new CommentsController(_commentService, _roleManager.Object, _currentUserProvider.Object)
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
            _postRepository
                .Setup(t => t.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Post
                {
                    PostStatusID = (int)PostStatusEnum.Approved
                });

            _commentRepository
                .Setup(t => t.CreateAsync(It.IsAny<Comment>()))
                .ReturnsAsync(1);

            //Act
            var actionResult = await _commentController.Post(new CommentCreationDTO());

            //Assert
            var result = actionResult as ObjectResult;
            Assert.NotNull(result);
            Assert.AreEqual(result.StatusCode, StatusCodes.Status201Created);
        }

        [Test]
        public void Post_Inexistent_ThrowsKeyNotFoundException()
        {
            //Arrange
            _postRepository
                .Setup(t => t.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Post
                {
                    PostStatusID = (int)PostStatusEnum.Rejected
                });

            //Act
            //Assert
            Assert.ThrowsAsync<Exception>(() => _commentController.Post(new CommentCreationDTO()));
        }

        [Test]
        public void Post_StatusDifferentOfApproved_ThrowsException()
        {
            //Arrange
            _postRepository
                .Setup(t => t.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Post { PostStatusID = 1 });

            //Act
            //Assert
            Assert.ThrowsAsync<Exception>(() => _commentController.Post(new CommentCreationDTO()));
        }

        [Test]
        public async Task GetAll_Ok()
        {
            //Arrange
            _commentRepository
                .Setup(t => t.GetAllAsync())
                .ReturnsAsync(new List<Comment> { new Comment() });

            //Act
            var actionResult = await _commentController.GetAll();

            //Assert
            var okObjectResult = actionResult as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var model = okObjectResult.Value as List<CommentDTO>;
            Assert.NotNull(model);
        }

        [Test]
        [TestCase(1)]
        [TestCase(10)]
        public async Task GetById_Ok(int id)
        {
            //Arrange
            _commentRepository
                .Setup(t => t.GetByIdAsync(id))
                .ReturnsAsync(new Comment { });

            //Act
            var actionResult = await _commentController.GetById(id);

            //Assert
            var okObjectResult = actionResult as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var model = okObjectResult.Value as CommentDTO;
            Assert.NotNull(model);
        }

        [Test]
        [TestCase(-1)]
        public void GetById_NegativeId_ThrowsKeyNotFoundException(int id)
        {
            //Arrange
            _commentRepository
                .Setup(t => t.GetByIdAsync(id))
                .ReturnsAsync(null, new TimeSpan(5));

            //Act
            //Assert
            Assert.ThrowsAsync<KeyNotFoundException>(() => _commentController.GetById(id));
        }

        [Test]
        public async Task Put_Ok()
        {
            //Arrange
            _commentRepository
                .Setup(t => t.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Comment { ApplicationUserID = 1});
            
            _currentUserProvider
                .Setup(t => t.GetCurrentUserAsync())
                .ReturnsAsync(new ApplicationUser { Id = 1 });
            //Act
            await _commentController.Put(1, new CommentUpdateDTO { });
        }

        [Test]
        public async Task Delete_Ok()
        {
            //Arrange
            _commentRepository
                .Setup(t => t.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Comment { ApplicationUserID = 1 });

            _currentUserProvider
                .Setup(t => t.GetCurrentUserAsync())
                .ReturnsAsync(new ApplicationUser { Id = 1 });
            //Act
            await _commentController.Delete(1);
        }
    }
}