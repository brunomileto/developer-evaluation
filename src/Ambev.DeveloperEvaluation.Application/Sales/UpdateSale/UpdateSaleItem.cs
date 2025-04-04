namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
}