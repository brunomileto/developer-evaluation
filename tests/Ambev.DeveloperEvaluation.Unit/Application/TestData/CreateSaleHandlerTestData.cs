using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios
/// for the CreateSaleHandler.
/// </summary>
public static class CreateSaleHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid CreateSaleItemCommand entries.
    /// Items will include:
    /// - Random ProductId
    /// - Valid ProductName
    /// - Reasonable UnitPrice
    /// - Valid Quantity (1-10)
    /// </summary>
    private static readonly Faker<CreateSaleItemCommand> itemFaker = new Faker<CreateSaleItemCommand>()
        .RuleFor(i => i.ProductId, f => Guid.NewGuid())
        .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
        .RuleFor(i => i.UnitPrice, f => f.Random.Decimal(5m, 100m))
        .RuleFor(i => i.Quantity, f => f.Random.Int(1, 10));

    /// <summary>
    /// Configures the Faker to generate valid CreateSaleCommand entries.
    /// The generated commands include:
    /// - Random Customer and Branch IDs
    /// - Valid denormalized names
    /// - SaleDate in the past
    /// - One or more valid sale items
    /// </summary>
    private static readonly Faker<CreateSaleCommand> saleFaker = new Faker<CreateSaleCommand>()
        .RuleFor(s => s.CustomerId, f => Guid.NewGuid())
        .RuleFor(s => s.CustomerName, f => f.Name.FullName())
        .RuleFor(s => s.BranchId, f => Guid.NewGuid())
        .RuleFor(s => s.BranchName, f => f.Company.CompanyName())
        .RuleFor(s => s.SaleDate, f => f.Date.PastOffset(1).UtcDateTime)
        .RuleFor(s => s.Items, f => itemFaker.Generate(f.Random.Int(1, 5)));

    /// <summary>
    /// Generates a valid CreateSaleCommand instance.
    /// </summary>
    /// <returns>A fully populated CreateSaleCommand with valid data.</returns>
    public static CreateSaleCommand GenerateValidCommand()
    {
        return saleFaker.Generate();
    }

    /// <summary>
    /// Generates a valid Sale entity using the domain factory.
    /// The entity includes one valid SaleItem.
    /// </summary>
    /// <returns>A Sale domain entity with business rules applied.</returns>
    public static Sale GenerateValidSale()
    {
        var faker = new Faker();
        var item = SaleItem.Create(
            productId: Guid.NewGuid(),
            productName: faker.Commerce.ProductName(),
            unitPrice: faker.Random.Decimal(10, 100),
            quantity: faker.Random.Int(1, 10),
            saleId: Guid.NewGuid()
        );

        return Sale.Create(
            customerId: Guid.NewGuid(),
            customerName: faker.Person.FullName,
            branchId: Guid.NewGuid(),
            branchName: faker.Company.CompanyName(),
            saleNumber: faker.Random.Replace("S-###########"),
            items: [item]
        );
    }
}
