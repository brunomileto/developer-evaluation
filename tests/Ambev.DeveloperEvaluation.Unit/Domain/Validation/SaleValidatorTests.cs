using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

/// <summary>
/// Contains unit tests for the <see cref="SaleValidator"/> class.
/// Tests include validation for required fields, date constraints, total amount,
/// item presence, and business rule specifications.
/// </summary>
public class SaleValidatorTests
{
    private readonly SaleValidator _validator;

    public SaleValidatorTests()
    {
        _validator = new SaleValidator();
    }

    [Fact(DisplayName = "Valid sale should pass validation")]
    public void Given_ValidSale_When_Validated_Then_ShouldNotHaveErrors()
    {
        // Arrange
        var sale = SaleTestData.GenerateSale(saleDate: DateTime.UtcNow.AddSeconds(-1));
        
        // Act
        var result = _validator.TestValidate(sale);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Empty sale number should fail validation")]
    public void Given_EmptySaleNumber_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData.GenerateSale(saleNumber: string.Empty);

        // Act
        var result = _validator.TestValidate(sale);

        // Act
        result.ShouldHaveValidationErrorFor(s => s.SaleNumber)
            .WithErrorMessage("Sale number is required.");
    }

    [Fact(DisplayName = "Future sale date should fail validation")]
    public void Given_FutureSaleDate_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData.GenerateSale(saleDate: DateTime.UtcNow.AddDays(1));
        
        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.SaleDate)
            .WithErrorMessage("Sale date cannot be in the future.");
    }

    [Fact(DisplayName = "Missing customer name should fail validation")]
    public void Given_EmptyCustomerName_When_Validated_Then_ShouldHaveError()
    {
        var sale = SaleTestData.GenerateSale(customerName: string.Empty);

        var result = _validator.TestValidate(sale);

        result.ShouldHaveValidationErrorFor(s => s.CustomerName)
            .WithErrorMessage("Customer name is required.");
    }

    [Fact(DisplayName = "Cancelled sale should fail validation")]
    public void Given_InactiveSale_When_Validated_Then_ShouldHaveError()
    {
        var sale = SaleTestData.GenerateSale(status: Status.Cancelled);

        var result = _validator.TestValidate(sale);

        result.ShouldHaveValidationErrorFor(s => s.Status)
            .WithErrorMessage("Sale must be active.");
    }

    [Fact(DisplayName = "Empty items list should fail validation")]
    public void Given_EmptyItems_When_Validated_Then_ShouldHaveError()
    {
        var sale = SaleTestData.GenerateSale(items: []);

        var result = _validator.TestValidate(sale);

        result.ShouldHaveValidationErrorFor(s => s.Items)
            .WithErrorMessage("At least one item is required in the sale.");
    }

    [Fact(DisplayName = "Negative total amount should fail validation")]
    public void Given_NegativeTotalAmount_When_Validated_Then_ShouldHaveError()
    {
        var sale = SaleTestData.GenerateSale();
        SaleTestData.SetTotalAmount(sale, -10m);

        var result = _validator.TestValidate(sale);

        result.ShouldHaveValidationErrorFor(s => s.TotalAmount)
            .WithErrorMessage("Total amount must be zero or positive.");
    }
}
