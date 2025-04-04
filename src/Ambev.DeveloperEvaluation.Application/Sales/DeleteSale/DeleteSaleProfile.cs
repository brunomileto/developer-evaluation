using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

public class DeleteSaleProfile : Profile
{
    public DeleteSaleProfile()
    {
        CreateMap<Guid, DeleteSaleCommand>();
    }
}