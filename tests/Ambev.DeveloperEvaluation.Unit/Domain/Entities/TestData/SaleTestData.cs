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
    public static Sale GenerateValidSale()
    {
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var items = new List<SaleItem>
        {
            GenerateValidSaleItem()
        };

        return Sale.Create(
            customerId: customerId,
            customerName: Faker.Person.FullName,
            branchId: branchId,
            branchName: Faker.Company.CompanyName(),
            saleNumber: Faker.Random.Replace("S-###########"),
            items: items
        );
    }

    /// <summary>
    /// Generates a sale with status set to Cancelled (invalid for active operations).
    /// </summary>
    /// <returns>A <see cref="Sale"/> instance with cancelled status.</returns>
    public static Sale GenerateCancelledSale()
    {
        var sale = GenerateValidSale();
        sale.Status = Status.Cancelled;
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
        var sale = GenerateValidSale();
        sale.RecalculateTotal();
        return sale;
    }
    
    public static void SetTotalAmount(Sale sale, decimal value)
    {
        typeof(Sale).GetProperty("TotalAmount")!.SetValue(sale, value);
    }

}
