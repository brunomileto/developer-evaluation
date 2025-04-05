using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods to generate test data for <see cref="SaleItem"/>.
/// Ensures valid and invalid data scenarios for unit testing.
/// </summary>
public static class SaleItemTestData
{
    private static readonly Faker Faker = new();

    /// <summary>
    /// Generates a valid <see cref="SaleItem"/> using the factory method.
    /// </summary>
    /// <param name="saleId">Optional sale ID; if not provided, a new GUID will be used.</param>
    /// <returns>A valid instance of <see cref="SaleItem"/>.</returns>
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
    /// Generates a <see cref="SaleItem"/> with invalid quantity (0).
    /// </summary>
    /// <returns>A <see cref="SaleItem"/> instance with invalid quantity.</returns>
    public static SaleItem GenerateInvalidQuantityItem()
    {
        var item = SaleItem.Create(
            productId: Guid.NewGuid(),
            productName: Faker.Commerce.ProductName(),
            unitPrice: Faker.Random.Decimal(10, 100),
            quantity: 1,
            saleId: Guid.NewGuid()
        );

        item.GetType().GetProperty(nameof(SaleItem.Quantity))!.SetValue(item, 0);
        item.CalculateTotal();
        return item;
    }

    /// <summary>
    /// Generates a <see cref="SaleItem"/> with quantity greater than allowed (more than 20).
    /// </summary>
    /// <returns>A tuple with the necessary parameters to test overflow quantity.</returns>
    public static (Guid ProductId, string ProductName, decimal UnitPrice, int Quantity, Guid SaleId) GenerateOverflowQuantityParams()
    {
        return (
            productId: Guid.NewGuid(),
            productName: Faker.Commerce.ProductName(),
            unitPrice: Faker.Random.Decimal(10, 100),
            quantity: 21, 
            saleId: Guid.NewGuid()
        );
    }
}
