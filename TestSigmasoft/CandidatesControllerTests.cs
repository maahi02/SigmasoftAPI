using Application.Common.Models;
using Application.Dtos;
using Domain.Models;
using Infrastructure.Content;
using Infrastructure.Content.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using SigmasoftAPI.Controllers.Content;

namespace TestSigmasoft
{
    public class CandidatesControllerTests
    {
        private readonly Mock<IRepository<Candidate>> _mockRepository;
        private readonly Mock<IMemoryCache> _mockCache;
        private readonly Mock<CandidateService> _mockCandidateService;
        private readonly CandidatesController _controller;

        public CandidatesControllerTests()
        {
            // Mock the dependencies
            _mockRepository = new Mock<IRepository<Candidate>>();
            _mockCache = new Mock<IMemoryCache>();

            _mockCache.Setup(cache => cache.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
           .Returns(false); // Simulate cache miss

            _mockCache.Setup(cache => cache.CreateEntry(It.IsAny<object>()))
                      .Returns(Mock.Of<ICacheEntry>()); // Simulate cache entry creation


            // Create CandidateService with mocked dependencies
            var candidateService = new CandidateService(_mockRepository.Object, _mockCache.Object);

            // Inject the CandidateService into the controller
            _controller = new CandidatesController(candidateService);
        }

        [Fact]
        public async Task AddOrUpdateCandidate_ShouldReturnOk_WhenModelIsValid()
        {
            // Arrange
            var candidateDto = new CandidateDto
            {
                Email = "test@tst.com",
                FirstName = "Test",
                LastName = "User",
                FreeTextComment = "test comment"
            };

            // Set up mock behavior for CreateOrUpdateAsync to return a success result
            _mockRepository.Setup(repo => repo.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((Candidate)null); // Simulate no existing candidate
            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Candidate>()))
                .Returns(Task.CompletedTask); // Simulate adding the candidate

            // Act
            var result = await _controller.AddOrUpdateCandidate(candidateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Check if the result is Ok
            var serviceResult = Assert.IsType<ServiceResult<Candidate>>(okResult.Value);
            Assert.True(serviceResult.Success);
            Assert.Equal("Candidate created successfully", serviceResult.Message);
        }


        [Fact]
        public async Task AddOrUpdateCandidate_ShouldReturnBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            var candidateDto = new CandidateDto(); // Invalid model (missing required properties)

            _controller.ModelState.AddModelError("Email", "Email is required");

            // Act
            var result = await _controller.AddOrUpdateCandidate(candidateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result); // Check if the result is BadRequest
            Assert.Equal(1, ((SerializableError)badRequestResult.Value).Count); // Check if the validation error is present
        }


        [Fact]
        public async Task GetAllCandidates_ShouldReturnOk_WhenCandidatesExist()
        {
            // Arrange
            var candidateList = new List<Candidate>
            {
                new Candidate { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
                new Candidate { FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" }
            };

            // Set up mock repository
            _mockRepository.Setup(repo => repo.GetAllAsync())
                           .ReturnsAsync(candidateList);

            // Act
            var result = await _controller.GetAllCandidates();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Check if the result is Ok
            var returnValue = Assert.IsType<ServiceResult<IEnumerable<CandidateGetModelDto>>>(okResult.Value);
            Assert.True(returnValue.Success);
            Assert.Equal(2, returnValue.Data.Count()); // Ensure the correct number of candidates is returned
        }


    }
}