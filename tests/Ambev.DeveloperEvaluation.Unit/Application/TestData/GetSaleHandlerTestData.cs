using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios
/// for the GetSaleHandler tests.
/// </summary>
public static class GetSaleHandlerTestData
{
    /// <summary>
    /// Shared Bogus Faker instance for consistent and localized fake data generation.
    /// </summary>
    private static readonly Faker Faker = new();

    /// <summary>
    /// Generates a valid <see cref="Sale"/> instance using domain factories.
    /// The sale includes:
    /// - Random customer and branch IDs
    /// - Valid denormalized names
    /// - One or more valid sale items
    /// - Business rules applied (e.g. total calculation, item discount)
    /// </summary>
    /// <returns>A fully valid <see cref="Sale"/> entity for unit testing.</returns>
    public static Sale GenerateValidSale()
    {
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var customerName = Faker.Person.FullName;
        var branchName = Faker.Company.CompanyName();

        var items = new List<SaleItem>
        {
            SaleItem.Create(
                productId: Guid.NewGuid(),
                productName: Faker.Commerce.ProductName(),
                unitPrice: Faker.Random.Decimal(10, 100),
                quantity: Faker.Random.Int(1, 5),
                saleId: Guid.NewGuid()
            )
        };

        return Sale.Create(customerId, customerName, branchId, branchName, items);
    }
}