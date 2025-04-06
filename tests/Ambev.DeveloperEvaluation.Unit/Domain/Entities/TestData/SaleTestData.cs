using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data using the Bogus library for the <see cref="Sale"/> entity.
/// This class ensures consistency across unit tests involving sales.
/// </summary>
public static class SaleTestData
{
    private static readonly Faker Faker = new();

    /// <summary>
    /// Generates a valid <see cref="SaleItem"/> with business rule-compliant values.
    /// </summary>
    /// <param name="saleId">The sale ID this item belongs to (optional).</param>
    /// <returns>A valid <see cref="SaleItem"/> instance.</returns>
    public static SaleItem GenerateValidSaleItem(Guid? saleId = null)
    {
        return SaleItem.Create(
            productId: Guid.NewGuid(),
            productName: Faker.Commerce.ProductName(),
            unitPrice: Faker.Random.Decimal(10, 100),
            quantity: Faker.Random.Int(1, 10),
            saleId: saleId ?? Guid.NewGuid()
        );
    }

    /// <summary>
    /// Generates a valid <see cref="Sale"/> with one or more valid items.
    /// </summary>
    /// <returns>A valid <see cref="Sale"/> instance.</returns>
    public static Sale GenerateSale(
        Guid? customerId = null,
        string? customerName = null,
        Guid? branchId = null,
        string? branchName = null,
        string? saleNumber = null,
        decimal? totalAmout = null,
        DateTime? saleDate = null,
        Status? status = null,
        List<SaleItem>? items = null)
    {
        
        var sale =  Sale.Create(
            customerId: customerId ?? Guid.NewGuid(),
            customerName: customerName ?? Faker.Person.FullName,
            branchId: branchId ?? Guid.NewGuid(),
            branchName: branchName ?? Faker.Company.CompanyName(),
            saleNumber: saleNumber ?? Faker.Random.Replace("S-###########"),
            items: (List<SaleItem>?)items ?? ([GenerateValidSaleItem()])
        );
        
        if (totalAmout is not null)
            SetTotalAmount(sale, totalAmout.GetValueOrDefault());
        if (saleDate is not null)
            SetCreatedAt(sale, saleDate.GetValueOrDefault());
        if (status is not null)
            SetStatus(sale, status.GetValueOrDefault());
        
        return sale;
    }

    /// <summary>
    /// Generates a sale with status set to Cancelled (invalid for active operations).
    /// </summary>
    /// <returns>A <see cref="Sale"/> instance with cancelled status.</returns>
    public static Sale GenerateCancelledSale()
    {
        var sale = GenerateSale();
        sale.Update(sale.CustomerName, sale.BranchName, Status.Cancelled, sale.Items);
        return sale;
    }

    /// <summary>
    /// Generates a valid list of <see cref="SaleItem"/> for multi-item sales.
    /// </summary>
    /// <param name="saleId">Optional sale ID for all items.</param>
    /// <returns>A list of valid <see cref="SaleItem"/>.</returns>
    public static List<SaleItem> GenerateValidItemList(Guid? saleId = null)
    {
        return
        [
            GenerateValidSaleItem(saleId),
            GenerateValidSaleItem(saleId)
        ];
    }

    /// <summary>
    /// Generates a sale with total manually recalculated for comparison tests.
    /// </summary>
    /// <returns>A <see cref="Sale"/> with known total amount calculated.</returns>
    public static Sale GenerateSaleWithCalculatedTotal()
    {
        var sale = GenerateSale();
        sale.RecalculateTotal();
        return sale;
    }
    
    public static void SetTotalAmount(Sale sale, decimal value)
    {
        typeof(Sale).GetProperty("TotalAmount")!.SetValue(sale, value);
    }
    
    public static void SetCreatedAt(Sale sale, DateTime date)
    {
        typeof(Sale).GetProperty("SaleDate")!.SetValue(sale, date);
    }
    
    public static void SetStatus(Sale sale, Status status)
    {
        typeof(Sale).GetProperty("Status")!.SetValue(sale, status);
    }

}
