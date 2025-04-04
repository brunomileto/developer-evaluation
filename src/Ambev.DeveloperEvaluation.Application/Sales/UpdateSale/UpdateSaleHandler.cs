using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {request.Id} not found");

        sale.CustomerName = request.CustomerName;
        sale.BranchName = request.BranchName;
        sale.Items.Clear();

        foreach (var saleItem in request.Items.Select(item => new SaleItem
                 {
                     ProductId = item.ProductId,
                     Quantity = item.Quantity,
                     UnitPrice = item.UnitPrice,
                 }))
        {
            saleItem.CalculateTotal();
            sale.Items.Add(saleItem);
        }

        sale.RecalculateTotal();
        
        await _saleRepository.UpdateAsync(sale, cancellationToken);

        return _mapper.Map<UpdateSaleResult>(sale);
    }
}