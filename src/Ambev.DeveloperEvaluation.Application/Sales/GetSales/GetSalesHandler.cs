using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Handles the retrieval of a paginated, filtered, and ordered list of sales
/// </summary>
public class GetSalesHandler : IRequestHandler<GetSalesQuery, GetSalesResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of <see cref="GetSalesHandler"/>
    /// </summary>
    /// <param name="saleRepository">The sales repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetSalesHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the incoming <see cref="GetSalesQuery"/> to retrieve paginated sales
    /// </summary>
    /// <param name="request">The sales query parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated and filtered list of sales</returns>
    public async Task<GetSalesResult> Handle(GetSalesQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _saleRepository.GetPagedRawAsync(
            page: request.Page,
            size: request.Size,
            order: request.Order,
            customerName: request.CustomerName,
            branchName: request.BranchName,
            status: request.Status,
            cancellationToken: cancellationToken
        );

        return new GetSalesResult
        {
            Items = _mapper.Map<List<GetSalesListItemResult>>(items),
            CurrentPage = request.Page,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.Size),
            TotalCount = totalCount
        };
    }
}