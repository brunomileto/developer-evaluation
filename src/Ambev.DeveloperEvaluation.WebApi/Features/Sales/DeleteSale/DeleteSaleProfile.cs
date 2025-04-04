using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;

/// <summary>
/// Mapping profile for DeleteSale feature
/// </summary>
public class DeleteSaleProfile : Profile
{
    public DeleteSaleProfile()
    {
        CreateMap<Guid, DeleteSaleCommand>(); // Mapeia diretamente o ID para o comando
    }
}