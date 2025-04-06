using Ambev.DeveloperEvaluation.Application.Common;
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
    private readonly IDomainEventDispatcher _eventDomainDispatcher;

    public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper, IDomainEventDispatcher domainDispatcher)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _eventDomainDispatcher = domainDispatcher;
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
        
        sale.Update(
            customerName: request.CustomerName,
            branchName: request.BranchName,
            status: request.Status,
            items: request.Items.Select(item => SaleItem.Create(
                item.ProductId,
                item.ProductName,
                item.UnitPrice,
                item.Quantity,
                sale.Id
                )).ToList()
            );
        
        var domainValidation = sale.Validate();
        if (!domainValidation.IsValid)
        {
            var failures = domainValidation.Errors
                .Select(e => new ValidationFailure(e.Error, e.Detail))
                .ToList();

            throw new ValidationException("Sale validation failed.", failures);
        }
        
        await _saleRepository.UpdateAsync(sale, cancellationToken);

        await _eventDomainDispatcher.DispatchEventsAsync(sale, cancellationToken);

        return _mapper.Map<UpdateSaleResult>(sale);
    }
}