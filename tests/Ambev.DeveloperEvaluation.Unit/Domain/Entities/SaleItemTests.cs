using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the <see cref="SaleItem"/> entity.
/// Validates calculation logic, business rules, and validation.
/// </summary>
public class SaleItemTests
{
    /// <summary>
    /// Tests that given valid item data, discount and total are calculated correctly.
    /// </summary>
    [Fact(DisplayName = "Given valid item data When calculating total Then computes discount and total correctly")]
    public void Given_ValidItem_When_CalculatingTotal_Then_ShouldComputeDiscountAndTotal()
    {
        // Arrange
        var item = SaleItem.Create(
            productId: Guid.NewGuid(),
            productName: "Product A",
            unitPrice: 100,
            quantity: 10, // triggers 20% discount
            saleId: Guid.NewGuid());

        // Act
        item.CalculateTotal();

        // Assert
        item.DiscountType.Should().Be(DiscountType.TwentyPercent);
        item.Discount.Should().Be(200); // 10 * 100 * 0.20
        item.Total.Should().Be(800);
    }

    /// <summary>
    /// Tests that creating a sale item with quantity over 20 results in validation error.
    /// </summary>
    [Fact(DisplayName = "Given excessive quantity When validating item Then returns invalid")]
    public void Given_TooManyItems_When_Validated_Then_ShouldBeInvalid()
    {
        // Arrange
        var (productId, productName, unitPrice, quantity, saleId) = SaleItemTestData.GenerateOverflowQuantityParams();
        var item = SaleItem.Create(productId, productName, unitPrice, quantity, saleId);

        // Act
        var result = item.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Detail == "Quantity cannot exceed 20.");
    }



    /// <summary>
    /// Tests that an item with quantity 0 is considered invalid.
    /// </summary>
    [Fact(DisplayName = "Given invalid quantity When checking validity Then returns false")]
    public void Given_InvalidQuantity_When_IsValidQuantity_Then_ReturnsFalse()
    {
        // Arrange
        var item = SaleItemTestData.GenerateInvalidQuantityItem();

        // Act
        var isValid = item.Validate().IsValid;

        // Assert
        isValid.Should().BeFalse();
    }

    /// <summary>
    /// Tests that a valid item passes validation.
    /// </summary>
    [Fact(DisplayName = "Given valid item data When validating Then returns valid result")]
    public void Given_ValidItem_When_Validated_Then_ShouldBeValid()
    {
        // Arrange
        var item = SaleItemTestData.GenerateValidSaleItem();

        // Act
        var result = item.Validate();

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}
