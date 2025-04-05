using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation.Results;

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

        List<SaleItem> newSaleItems = [];
        
        foreach (var saleItem in request.Items)
        {
            var updatedSaleItem = SaleItem.Create(
                saleItem.ProductId,
                saleItem.ProductName,
                saleItem.UnitPrice,
                saleItem.Quantity,
                sale.Id);
            newSaleItems.Add(updatedSaleItem);
        }

        var updatedSale = Sale.Create(
            id: request.Id,
            customerId: sale.CustomerId,
            customerName: request.CustomerName,
            branchId: sale.BranchId,
            branchName: request.BranchName,
            saleNumber: sale.SaleNumber,
            items: newSaleItems
        );
        
        var domainValidation = updatedSale.Validate();
        if (!domainValidation.IsValid)
        {
            var failures = domainValidation.Errors
                .Select(e => new ValidationFailure(e.Error, e.Detail))
                .ToList();

            throw new ValidationException("Sale validation failed.", failures);
        }
        
        await _saleRepository.UpdateAsync(sale, cancellationToken);

        return _mapper.Map<UpdateSaleResult>(sale);
    }
}