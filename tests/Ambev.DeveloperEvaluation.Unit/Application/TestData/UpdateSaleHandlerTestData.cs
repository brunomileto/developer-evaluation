using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation for UpdateSaleHandler unit tests
/// to ensure consistency and realistic scenarios.
/// </summary>
public static class UpdateSaleHandlerTestData
{
    /// <summary>
    /// Generates a valid <see cref="UpdateSaleCommand"/> with populated item data.
    /// Ensures the data satisfies all business rules and validation requirements.
    /// </summary>
    /// <returns>A valid <see cref="UpdateSaleCommand"/> instance.</returns>
    public static UpdateSaleCommand GenerateValidCommand()
    {
        var faker = new Faker();
        return new UpdateSaleCommand
        {
            Id = Guid.NewGuid(),
            CustomerName = faker.Name.FullName(),
            BranchName = faker.Company.CompanyName(),
            Items = new List<UpdateSaleItem>
            {
                new()
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = faker.Commerce.ProductName(),
                    UnitPrice = faker.Random.Decimal(10, 100),
                    Quantity = 5,
                }
            }
        };
    }

    /// <summary>
    /// Generates an existing <see cref="Sale"/> entity with the specified ID.
    /// Uses the domain factory to apply business rules and calculate totals.
    /// </summary>
    /// <param name="id">The specific ID to assign to the generated sale.</param>
    /// <returns>An existing <see cref="Sale"/> entity with populated items.</returns>
    public static Sale GenerateExistingSale(Guid id)
    {
        var faker = new Faker();

        var items = new List<SaleItem>
        {
            SaleItem.Create(
                Guid.NewGuid(),
                faker.Commerce.ProductName(),
                faker.Random.Decimal(10, 100),
                5,
                id)
        };

        var sale = Sale.Create(
            customerId: Guid.NewGuid(),
            customerName: faker.Name.FullName(),
            branchId: Guid.NewGuid(),
            branchName: faker.Company.CompanyName(),
            items: items
        );

        sale.Id = id;

        return sale;
    }
}
