using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

/// <summary>
/// Unit tests for <see cref="SaleItemValidator"/> validation rules.
/// </summary>
public class SaleItemValidatorTests
{
    private readonly SaleItemValidator _validator;

    public SaleItemValidatorTests()
    {
        _validator = new SaleItemValidator();
    }

    [Fact(DisplayName = "Valid sale item should pass validation")]
    public void Given_ValidSaleItem_When_Validated_Then_ShouldBeValid()
    {
        var item = SaleItemTestData.GenerateValidSaleItem();

        var result = _validator.TestValidate(item);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Missing product ID should fail validation")]
    public void Given_EmptyProductId_When_Validated_Then_ShouldHaveError()
    {
        var item = SaleItemTestData.GenerateValidSaleItem();
        item.ProductId = Guid.Empty;

        var result = _validator.TestValidate(item);

        result.ShouldHaveValidationErrorFor(i => i.ProductId)
              .WithErrorMessage("Product ID is required.");
    }

    [Fact(DisplayName = "Missing product name should fail validation")]
    public void Given_EmptyProductName_When_Validated_Then_ShouldHaveError()
    {
        var item = SaleItemTestData.GenerateValidSaleItem();
        item.ProductName = "";

        var result = _validator.TestValidate(item);

        result.ShouldHaveValidationErrorFor(i => i.ProductName)
              .WithErrorMessage("Product name is required.");
    }

    [Fact(DisplayName = "Product name exceeding max length should fail validation")]
    public void Given_TooLongProductName_When_Validated_Then_ShouldHaveError()
    {
        var item = SaleItemTestData.GenerateValidSaleItem();
        item.ProductName = new string('X', 101); // >100

        var result = _validator.TestValidate(item);
        
        result.Errors
            .Should()
            .ContainSingle(e =>
                e.PropertyName == nameof(SaleItem.ProductName) &&
                e.ErrorMessage.Contains("must be 100 characters or fewer"));

    }

    [Theory(DisplayName = "Invalid quantity should fail validation")]
    [InlineData(0, "Quantity must be greater than zero.")]
    [InlineData(21, "Quantity cannot exceed 20.")]
    public void Given_InvalidQuantity_When_Validated_Then_ShouldHaveError(int quantity, string expectedMessage)
    {
        var item = SaleItemTestData.GenerateValidSaleItem();
        item.Quantity = quantity;

        var result = _validator.TestValidate(item);

        result.ShouldHaveValidationErrorFor(i => i.Quantity)
              .WithErrorMessage(expectedMessage);
    }

    [Fact(DisplayName = "Negative unit price should fail validation")]
    public void Given_NegativeUnitPrice_When_Validated_Then_ShouldHaveError()
    {
        var item = SaleItemTestData.GenerateValidSaleItem();
        item.UnitPrice = -5;

        var result = _validator.TestValidate(item);

        result.ShouldHaveValidationErrorFor(i => i.UnitPrice)
              .WithErrorMessage("Unit price must be greater than 0.");
    }

    [Fact(DisplayName = "Negative discount should fail validation")]
    public void Given_NegativeDiscount_When_Validated_Then_ShouldHaveError()
    {
        var item = SaleItemTestData.GenerateValidSaleItem();

        typeof(SaleItem).GetProperty("Discount")!
            .SetValue(item, -5m);

        var result = _validator.TestValidate(item);

        result.ShouldHaveValidationErrorFor(i => i.Discount)
              .WithErrorMessage("Discount must be zero or positive.");
    }

    [Fact(DisplayName = "Negative total should fail validation")]
    public void Given_NegativeTotal_When_Validated_Then_ShouldHaveError()
    {
        var item = SaleItemTestData.GenerateValidSaleItem();

        typeof(SaleItem).GetProperty("Total")!
            .SetValue(item, -100m);

        var result = _validator.TestValidate(item);

        result.ShouldHaveValidationErrorFor(i => i.Total)
              .WithErrorMessage("Total must be zero or positive.");
    }
}
