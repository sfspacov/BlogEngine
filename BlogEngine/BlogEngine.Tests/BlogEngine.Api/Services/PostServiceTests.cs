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

namespace BlogEngine.Tests.BlogEngine.Api.Services
{
    [TestFixture]
    public class PostServiceTests
    {
        private Mock<IPostRepository> _postRepository;
        private Mock<IPostStatusRepository> _postStatusRepository;
        private Mock<ICurrentUserProvider> _currentUserProvider;
        private IReadingTimeEstimator _readingTimeEstimator;
        private IMapper _mapper;
        private PostService _postService;
        [SetUp]
        public void SetUp()
        {

            var myProfile = new AutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            _postRepository = new Mock<IPostRepository>();
            _postStatusRepository = new Mock<IPostStatusRepository>();
            _currentUserProvider = new Mock<ICurrentUserProvider>();
            _readingTimeEstimator = new ReadingTimeEstimator();
            _mapper = new Mapper(configuration);
            _postService = new PostService(_postRepository.Object, _postStatusRepository.Object, _mapper, _readingTimeEstimator, _currentUserProvider.Object);
        }

        [Test]

        public async Task CreateAsync_Ok()
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
            var actual = await _postService.CreateAsync(
                new PostCreationDTO
                {
                    Content = "Be or not be, that's the question",
                    Title = "Hamlet"
                });

            //Assert
            Assert.NotZero(actual);
            var expect = 1;
            Assert.AreEqual(expect, actual);
        }

        [Test]

        public void CreateAsync_NullValues_ThrowArgumentNullException()
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
            //Assert
            Assert.That(async () => await _postService.CreateAsync(
                new PostCreationDTO
                {
                    Content = null,
                    Title = null
                }),
                Throws.ArgumentNullException);
        }

        [Test]
        [TestCase(null)]
        [TestCase("PendingApproval")]
        [TestCase("Approved")]
        [TestCase("Rejected")]
        public async Task GetAllAsync_Ok(string status)
        {
            //Arrange
            _postRepository
                .Setup(t => t.GetAllAsync(status))
                .ReturnsAsync(new List<Post>() { new Post { } });

            //Act
            var actual = await _postService.GetAllAsync(status);

            //Assert
            Assert.NotZero(actual.Count);
        }

        [Test]
        [TestCase("")]
        [TestCase("Completed")]
        public async Task GetAllAsync_StatusFake(string status)
        {
            //Arrange
            _postRepository
                .Setup(t => t.GetAllAsync(status))
                .ReturnsAsync(new List<Post>() { });

            //Act
            var actual = await _postService.GetAllAsync(status);

            //Assert
            var expect = 0;
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.Count, expect);
        }

        [Test]
        [TestCase(1)]
        public async Task GetByIdAsync_Ok(int id)
        {
            //Arrange
            _postRepository
                .Setup(t => t.GetByIdAsync(id))
                .ReturnsAsync(new Post { });

            //Act
            var actual = await _postService.GetByIdAsync(id);

            //Assert
            Assert.IsNotNull(actual);
        }

        [Test]
        [TestCase(-1)]
        public void GetByIdAsync_NegativeId_ThrowsKeyNotFoundException(int id)
        {
            //Arrange
            _postRepository
                .Setup(t => t.GetByIdAsync(id))
                .ReturnsAsync(null, new TimeSpan(5));

            //Act
            //Assert
            Assert.ThrowsAsync<KeyNotFoundException>(() => _postService.GetByIdAsync(id));
        }

        [Test]
        public async Task UpdateAsync_Ok()
        {
            //Arrange
            _postRepository
                .Setup(t => t.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Post { ID = 1, Title = "Pinocchio", Content = "Once upon a time..." });

            _postStatusRepository
                .Setup(t => t.GetByDescription(It.IsAny<string>()))
                .ReturnsAsync(new PostStatus());

            //Act
            await _postService.UpdateAsync(1, new PostCreationDTO { Content = "Bla, bla, bla", Title = "Small talk" });
        }


        [Test]
        public void UpdateAsync_ContenttNull_ThrowsArgumentNullException()
        {
            //Arrange
            _postRepository
                .Setup(t => t.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Post { ID = 1, Title = "Pinocchio", Content = "Once upon a time..." });

            //Act
            Assert.ThrowsAsync<ArgumentNullException>(() => _postService.UpdateAsync(1, new PostCreationDTO { Content = null, Title = "Small talk" }));
        }

        [Test]
        public async Task DeleteAsync_Ok()
        {
            //Arrange
            _postRepository
                .Setup(t => t.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Post { ID = 1, Title = "Pinocchio", Content = "Once upon a time..." });

            //Act
            await _postService.DeleteAsync(1);
        }

        [Test]
        public void DeleteAsync_InexistentPost_ThrowsArgumentException()
        {
            //Arrange
            _postRepository
                .Setup(t => t.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(null, new TimeSpan(5));

            //Act
            Assert.ThrowsAsync<ArgumentException>(() => _postService.DeleteAsync(-1));
        }
    }
}