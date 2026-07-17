using ECommerce.Domain.Contracts.Persistence;
using ECommerce.Application.Contracts.Infrastructure;
using ECommerce.Application.Features.Orders.Commands;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;
using Moq;

namespace ECommerce.Tests.Orders;

public class PlaceOrderCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly Mock<IProductRepository> _productRepoMock = new();
    private readonly Mock<IOrderRepository> _orderRepoMock = new();
    private readonly Mock<IStockServiceClient> _stockServiceMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly PlaceOrderCommandHandler _handler;

    public PlaceOrderCommandHandlerTests()
    {
        _stockServiceMock
            .Setup(s => s.ReserveAsync(It.IsAny<List<ReserveItemDto>>(), default))
            .ReturnsAsync(new ReserveResponseDto { Reserved = true });

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(default)).Returns(Task.CompletedTask);
        _handler = new PlaceOrderCommandHandler(_userRepoMock.Object, _productRepoMock.Object, _orderRepoMock.Object, _stockServiceMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_CreatesOrder_WhenValid()
    {
        var userId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        var user = new User("cliente@test.com", "Cliente", "hash", "Customer");
        var product = Product.New("Laptop", "Descripción", 1500m, 10, categoryId);

        _userRepoMock.Setup(r => r.GetByIdAsync(userId, default)).ReturnsAsync(user);
        _productRepoMock.Setup(r => r.GetByIdAsync(productId, default)).ReturnsAsync(product);
        _orderRepoMock.Setup(r => r.AddAsync(It.IsAny<Order>(), default)).Returns(Task.CompletedTask);

        var command = new PlaceOrderCommand(userId, [new OrderLine(productId, 2)]);

        var result = await _handler.Handle(command, default);

        Assert.Equal(userId, result.UserId);
        Assert.Equal("Pending", result.Status);
        _orderRepoMock.Verify(r => r.AddAsync(It.IsAny<Order>(), default), Times.Once);
    }

    [Fact]
    public async Task Handle_ThrowsBusinessException_WhenNoLines()
    {
        var userId = Guid.NewGuid();
        var user = new User("cliente@test.com", "Cliente", "hash", "Customer");
        _userRepoMock.Setup(r => r.GetByIdAsync(userId, default)).ReturnsAsync(user);

        var command = new PlaceOrderCommand(userId, []);

        await Assert.ThrowsAsync<BusinessException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenUserNotFound()
    {
        var userId = Guid.NewGuid();
        _userRepoMock.Setup(r => r.GetByIdAsync(userId, default)).ReturnsAsync((User?)null);

        var command = new PlaceOrderCommand(userId, [new OrderLine(Guid.NewGuid(), 1)]);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
    }
}
