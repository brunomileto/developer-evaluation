using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Unit tests for <see cref="GetSalesHandler"/>.
/// </summary>
public class GetSalesHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly GetSalesHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSalesHandlerTests"/> class.
    /// Sets up dependencies and the handler under test.
    /// </summary>
    public GetSalesHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetSalesHandler(_saleRepository, _mapper);
    }

    /// <summary>
    /// Tests that a valid query returns a paginated list of sales with the expected total count and page information.
    /// </summary>
    [Fact(DisplayName = "Given valid query When getting sales Then returns paginated list")]
    public async Task Handle_ValidQuery_ReturnsPaginatedSales()
    {
        // Given
        var sales = Enumerable.Range(0, 3)
            .Select(_ => GetSaleHandlerTestData.GenerateValidSale())
            .ToList();

        var query = new GetSalesQuery
        {
            Page = 1,
            Size = 10,
            Order = "SaleDate desc",
            CustomerName = null,
            BranchName = null,
            Status = null
        };

        var totalCount = 3;

        var mapped = sales.Select(s => new GetSalesListItemResult
        {
            Id = s.Id,
            SaleNumber = s.SaleNumber,
            SaleDate = s.SaleDate,
            CustomerName = s.CustomerName,
            BranchName = s.BranchName,
            TotalAmount = s.TotalAmount,
            Status = s.Status.ToString()
        }).ToList();

        _saleRepository.GetPagedRawAsync(
            query.Page, query.Size, query.Order,
            query.CustomerName, query.BranchName, query.Status,
            Arg.Any<CancellationToken>()).Returns((sales, totalCount));

        _mapper.Map<List<GetSalesListItemResult>>(sales).Returns(mapped);

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(sales.Count);
        result.TotalCount.Should().Be(totalCount);
        result.CurrentPage.Should().Be(query.Page);
        result.TotalPages.Should().Be(1); // 3 items / 10 per page = 1 page

        await _saleRepository.Received(1).GetPagedRawAsync(
            query.Page, query.Size, query.Order,
            query.CustomerName, query.BranchName, query.Status,
            Arg.Any<CancellationToken>());
    }
}
