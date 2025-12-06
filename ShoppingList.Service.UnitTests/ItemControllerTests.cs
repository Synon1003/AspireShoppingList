using Moq;
using AutoFixture;
using FluentAssertions;

using ShoppingList.Core.Domain.Entities;
using ShoppingList.Core.Domain.RepositoryContracts;
using ShoppingList.Service.Controllers;
using ShoppingList.Core.Dtos;
using ShoppingList.Core.Mappings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ShoppingList.Service.UnitTests;

public class ItemContollerTests
{
    private IRepository<Item> _repository;
    private ILogger<ItemController> _logger;
    private Mock<ILogger<ItemController>> _mockLogger;
    private Mock<IRepository<Item>> _mockMongoRepository;

    private Fixture _fixture;


    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _mockMongoRepository = new Mock<IRepository<Item>>();
        _mockLogger = new Mock<ILogger<ItemController>>();
        _repository = _mockMongoRepository.Object;
        _logger = _mockLogger.Object;
    }

    [Test]
    public async Task GetAllAsync_WithAllItems_ReturnsOk()
    {
        // Arrange
        var itemsResponseList = _fixture.Create<List<Item>>();
        ItemController _controller = new ItemController(_repository, _logger);

        _mockMongoRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(itemsResponseList);

        // Act
        var result = await _controller.GetAllAsync();
        var okResult = result as OkObjectResult;

        // Assert
        okResult.Should().BeOfType<OkObjectResult>();
        okResult.Value.Should().BeEquivalentTo(itemsResponseList.Select(item => item.ToDto()));
    }

    [Test]
    public async Task GetAsync_WithExistingItem_ReturnsOk()
    {
        // Arrange
        var itemResponse = _fixture.Create<Item>();
        ItemController _controller = new ItemController(_repository, _logger);

        _mockMongoRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(itemResponse);

        // Act
        var result = await _controller.GetAsync(itemResponse.Id);
        var okResult = result as OkObjectResult;

        // Assert
        okResult.Should().BeOfType<OkObjectResult>();
        okResult.Value.Should().BeEquivalentTo(itemResponse.ToDto());
    }

    [Test]
    public async Task GetAsync_WithItemDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        ItemController _controller = new ItemController(_repository, _logger);

        _mockMongoRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as Item);

        // Act
        var result = await _controller.GetAsync(Guid.NewGuid());
        var notFoundResult = result as NotFoundResult;

        // Assert
        notFoundResult.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task AddAsync_WithValidItem_ReturnsCreated()
    {
        // Arrange
        var createItemDto = _fixture.Create<CreateItemDto>();
        ItemController _controller = new ItemController(_repository, _logger);

        _mockMongoRepository.Setup(repo => repo.InsertAsync(It.IsAny<Item>()))
            .Verifiable();

        // Act
        var result = await _controller.AddAsync(createItemDto);
        var createdResult = result as CreatedAtActionResult;

        // Assert
        createdResult.Should().BeOfType<CreatedAtActionResult>();
        createdResult.ActionName.Should().Be(nameof(_controller.GetAsync));
    }

    public static readonly IEnumerable<object[]> itemStatusTestCases = new[]
    {
        new object[] { ItemStatus.NotPurchased },
        new object[] { ItemStatus.Purchased }
    };

    [Test]
    [TestCaseSource(nameof(itemStatusTestCases))]
    public async Task PatchAsync_WithExistingtem_TogglesStatusAndReturnsNoContent(ItemStatus status)
    {
        // Arrange
        var expectedStatus = status;
        var item = _fixture.Build<Item>().With(i => i.Status, expectedStatus).Create();
        ItemController _controller = new ItemController(_repository, _logger);

        _mockMongoRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(item);

        _mockMongoRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Item>()))
            .Verifiable();

        // Act
        var result = await _controller.PatchAsync(item.Id);
        var noContentResult = result as NoContentResult;

        // Assert
        noContentResult.Should().BeOfType<NoContentResult>();
        item.Status.Should().NotBe(expectedStatus);
    }

    [Test]
    public async Task PatchAsync_ItemDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        ItemController _controller = new ItemController(_repository, _logger);

        _mockMongoRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as Item);

        // Act
        var result = await _controller.PatchAsync(Guid.NewGuid());
        var notFoundResult = result as NotFoundResult;

        // Assert
        notFoundResult.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task DeleteAsync_WithExistingItem_DeletesAndReturnsNoContent()
    {
        // Arrange
        var item = _fixture.Create<Item>();
        ItemController _controller = new ItemController(_repository, _logger);

        _mockMongoRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(item);

        _mockMongoRepository.Setup(repo => repo.RemoveAsync(It.IsAny<Guid>()))
            .Verifiable();

        // Act
        var result = await _controller.DeleteAsync(item.Id);
        var noContentResult = result as NoContentResult;

        // Assert
        noContentResult.Should().BeOfType<NoContentResult>();
    }

    [Test]
    public async Task DeleteAsync_ItemDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        ItemController _controller = new ItemController(_repository, _logger);

        _mockMongoRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as Item);

        // Act
        var result = await _controller.DeleteAsync(Guid.NewGuid());
        var notFoundResult = result as NotFoundResult;

        // Assert
        notFoundResult.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task InitializeAsync_WithItemsToInitialize_ReturnsNoContent()
    {
        // Arrange
        var createItemDtos = _fixture.Create<List<CreateItemDto>>();
        ItemController _controller = new ItemController(_repository, _logger);

        _mockMongoRepository.Setup(repo => repo.InitializeAsync(It.IsAny<List<Item>>()))
            .Verifiable();

        // Act
        var result = await _controller.InitializeAsync(createItemDtos);
        var notFoundResult = result as NoContentResult;

        // Assert
        notFoundResult.Should().BeOfType<NoContentResult>();
    }

    [Test]
    public async Task ClearAsync_ReturnsNoContent()
    {
        // Arrange
        ItemController _controller = new ItemController(_repository, _logger);

        _mockMongoRepository.Setup(repo => repo.ClearAsync())
            .Verifiable();

        // Act
        var result = await _controller.ClearAsync();
        var noContentResult = result as NoContentResult;

        // Assert
        noContentResult.Should().BeOfType<NoContentResult>();
    }
}
