using BlogEngine.Shared.DTOs.Comment;
using NUnit.Framework;
using Moq;
using BlogEngine.Core.Services.Abstractions;
using BlogEngine.Api.Services.Implementations;
using AutoMapper;
using System.Threading.Tasks;
using BlogEngine.Core.Data.Entities;
using BlogEngine.Api.Common.Mappings;
using BlogEngine.Api.Services.Abstractions.Identity;
using BlogEngine.Shared.DTOs.Blog;
using System.Collections.Generic;
using System;

namespace BlogEngine.Tests.BlogEngine.Api.Services
{
    [TestFixture]
    public class CommentServiceTests
    {
        private Mock<IPostRepository> _postRepository;
        private Mock<IAccountService> _accountService;
        private Mock<ICommentRepository> _commentRepository;
        private Mock<ICurrentUserProvider> _currentUserProvider;
        private IMapper _mapper;
        private CommentService _commentService;
        [SetUp]
        public void SetUp()
        {

            var myProfile = new AutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            _postRepository = new Mock<IPostRepository>();
            _accountService = new Mock<IAccountService>();
            _commentRepository = new Mock<ICommentRepository>();
            _currentUserProvider = new Mock<ICurrentUserProvider>();
            _mapper = new Mapper(configuration);
            _commentService = new CommentService(_postRepository.Object, _commentRepository.Object, _currentUserProvider.Object, _accountService.Object, _mapper);
        }

        [Test]
        public async Task CreateAsync_Ok()
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
            var actual = await _commentService.CreateAsync(new CommentCreationDTO());

            //Assert
            Assert.NotZero(actual);
            var expect = 1;
            Assert.AreEqual(expect, actual);
        }

        [Test]
        public void CreateAsync_PostInexistent_ThrowsKeyNotFoundException()
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
            Assert.ThrowsAsync<Exception>(() => _commentService.CreateAsync(new CommentCreationDTO()));
        }

        [Test]
        public void CreateAsync_StatusDifferentOfApproved_ThrowsException()
        {
            //Arrange
            _postRepository
                .Setup(t => t.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(null, new System.TimeSpan(5));

            //Act
            //Assert
            Assert.ThrowsAsync<KeyNotFoundException>(() => _commentService.CreateAsync(new CommentCreationDTO()));
        }

        [Test]
        public async Task GetAllAsync_Ok()
        {
            //Arrange
            _commentRepository
                .Setup(t => t.GetAllAsync())
                .ReturnsAsync(new List<Comment> { new Comment() });

            //Act
            var actual = await _commentService.GetAllAsync();

            //Assert
            Assert.NotZero(actual.Count);
        }

        [Test]
        [TestCase(1)]
        public async Task GetByIdAsync_Ok(int id)
        {
            //Arrange
            _commentRepository
                .Setup(t => t.GetByIdAsync(id))
                .ReturnsAsync(new Comment { });

            //Act
            var actual = await _commentService.GetByIdAsync(id);

            //Assert
            Assert.IsNotNull(actual);
        }

        [Test]
        [TestCase(-1)]
        public void GetByIdAsync_NegativeId_ThrowsKeyNotFoundException(int id)
        {
            //Arrange
            _commentRepository
                .Setup(t => t.GetByIdAsync(id))
                .ReturnsAsync(null, new TimeSpan(5));

            //Act
            //Assert
            Assert.ThrowsAsync<KeyNotFoundException>(() => _commentService.GetByIdAsync(id));
        }

        [Test]
        public async Task UpdateAsync_Ok()
        {
            //Arrange
            _commentRepository
                .Setup(t => t.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Comment { });

            //Act
            await _commentService.UpdateAsync(1, new CommentUpdateDTO { });
        }


        [Test]
        public void UpdateAsync_InexistentComment_ThrowsArgumentException()
        {
            //Arrange
            _commentRepository
                .Setup(t => t.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(null, new TimeSpan(5));

            //Act
            //Assert
            Assert.ThrowsAsync<ArgumentException>(() => _commentService.UpdateAsync(1, new CommentUpdateDTO()));
        }

        [Test]
        public async Task DeleteAsync_Ok()
        {
            //Arrange
            _commentRepository
                .Setup(t => t.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Comment());

            //Act
            await _commentService.DeleteAsync(1);
        }

        [Test]
        public void DeleteAsync_InexistentComment_ThrowsArgumentException()
        {
            //Arrange
            _commentRepository
                .Setup(t => t.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(null, new TimeSpan(5));

            //Act
            Assert.ThrowsAsync<ArgumentException>(() => _commentService.DeleteAsync(-1));
        }
    }
}