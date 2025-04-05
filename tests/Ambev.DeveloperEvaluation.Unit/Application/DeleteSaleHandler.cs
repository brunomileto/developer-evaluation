using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="DeleteSaleHandler"/> class.
/// </summary>
public class DeleteSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly DeleteSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteSaleHandlerTests"/> class.
    /// Sets up test dependencies using mocked repository and mapper.
    /// </summary>
    public DeleteSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new DeleteSaleHandler(_saleRepository, _mapper);
    }

    /// <summary>
    /// Tests that when a valid sale ID is passed,
    /// the sale is deleted successfully and a result is returned.
    /// </summary>
    [Fact(DisplayName = "Given valid sale ID When deleting Then returns success response")]
    public async Task Handle_ValidId_ReturnsSuccessResponse()
    {
        // Given
        var saleId = Guid.NewGuid();
        var command = new DeleteSaleCommand(saleId);

        _saleRepository.DeleteAsync(saleId, Arg.Any<CancellationToken>()).Returns(true);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(saleId);
        await _saleRepository.Received(1).DeleteAsync(saleId, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that when a non-existing sale ID is passed,
    /// a <see cref="KeyNotFoundException"/> is thrown indicating the sale was not found.
    /// </summary>
    [Fact(DisplayName = "Given non-existing sale ID When deleting Then throws KeyNotFoundException")]
    public async Task Handle_InvalidId_ThrowsKeyNotFoundException()
    {
        // Given
        var saleId = Guid.NewGuid();
        var command = new DeleteSaleCommand(saleId);

        _saleRepository.DeleteAsync(saleId, Arg.Any<CancellationToken>()).Returns(false);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {saleId} not found");
    }
}
