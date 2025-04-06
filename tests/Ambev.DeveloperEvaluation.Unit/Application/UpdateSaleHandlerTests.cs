using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Unit tests for <see cref="UpdateSaleHandler"/>
/// </summary>
public class UpdateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly UpdateSaleHandler _handler;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSaleHandlerTests"/> class.
    /// Sets up dependencies and the handler under test.
    /// </summary>
    public UpdateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _domainEventDispatcher = Substitute.For<IDomainEventDispatcher>();
        _handler = new UpdateSaleHandler(_saleRepository, _mapper, _domainEventDispatcher);
    }

    /// <summary>
    /// Tests that a valid update request returns the updated sale result.
    /// </summary>
    [Fact(DisplayName = "Given valid sale update When handling Then returns updated result")]
    public async Task Handle_ValidRequest_ReturnsUpdatedSale()
    {
        // Given
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();
        var existingSale = UpdateSaleHandlerTestData.GenerateExistingSale(command.Id);
        existingSale.Id = command.Id;
        var result = new UpdateSaleResult { Id = existingSale.Id };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(existingSale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(existingSale);
        _mapper.Map<UpdateSaleResult>(Arg.Any<Sale>()).Returns(result);

        // When
        var response = await _handler.Handle(command, CancellationToken.None);

        // Then
        response.Should().NotBeNull();
        response.Id.Should().Be(command.Id);
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid update request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid update request When handling Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidation()
    {
        // Given
        var command = new UpdateSaleCommand(); // Missing required fields

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Tests that attempting to update a non-existent sale throws a key not found exception.
    /// </summary>
    [Fact(DisplayName = "Given non-existent sale ID When updating Then throws not found exception")]
    public async Task Handle_SaleNotFound_ThrowsKeyNotFound()
    {
        // Given
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();
        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((Sale?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {command.Id} not found");
    }

    /// <summary>
    /// Tests that a sale with invalid item quantity throws a validation exception during update.
    /// </summary>
    [Fact(DisplayName = "Given sale with invalid quantity When updating Then throws validation exception")]
    public async Task Handle_InvalidItemQuantity_ThrowsValidation()
    {
        // Given
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();
        command.Items[0].Quantity = 0;

        var existingSale = UpdateSaleHandlerTestData.GenerateExistingSale(command.Id);

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(existingSale);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*quantity*");
    }
    
    /// <summary>
    /// Tests that domain events are dispatched after a successful sale update.
    /// </summary>
    [Fact(DisplayName = "Given valid sale update When handling Then dispatches domain events")]
    public async Task Handle_ValidRequest_DispatchesDomainEvents()
    {
        // Given
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();
        var existingSale = UpdateSaleHandlerTestData.GenerateExistingSale(command.Id);
        existingSale.Id = command.Id;
        var result = new UpdateSaleResult { Id = existingSale.Id };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(existingSale);
        _saleRepository.UpdateAsync(existingSale, Arg.Any<CancellationToken>()).Returns(existingSale);
        _mapper.Map<UpdateSaleResult>(existingSale).Returns(result);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _domainEventDispatcher.Received(1).DispatchEventsAsync(existingSale, Arg.Any<CancellationToken>());
    }

}
