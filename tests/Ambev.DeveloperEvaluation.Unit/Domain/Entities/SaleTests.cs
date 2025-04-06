using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the <see cref="Sale"/> entity class.
/// Tests include validation, status checking, and total calculation.
/// </summary>
public class SaleTests
{
    /// <summary>
    /// Tests that a newly created sale is considered active.
    /// </summary>
    [Fact(DisplayName = "Sale should be active on default creation")]
    public void Given_SaleWithActiveStatus_When_CheckingIsActive_Then_ShouldReturnTrue()
    {
        // Arrange
        var sale = SaleTestData.GenerateSale();

        // Act
        var isActive = sale.IsActive();

        // Assert
        Assert.True(isActive);
    }

    /// <summary>
    /// Tests that a sale with a non-active status is not considered active.
    /// </summary>
    [Fact(DisplayName = "Sale should not be active when status is not Active")]
    public void Given_SaleWithCancelledStatus_When_CheckingIsActive_Then_ShouldReturnFalse()
    {
        // Arrange
        var sale = SaleTestData.GenerateCancelledSale();
        
        // Act
        var isActive = sale.IsActive();

        // Assert
        Assert.False(isActive);
    }

    /// <summary>
    /// Tests that validation passes when the sale has valid data.
    /// </summary>
    [Fact(DisplayName = "Validation should pass for valid sale")]
    public void Given_ValidSale_When_Validated_Then_ShouldBeValid()
    {
        // Arrange
        var sale = SaleTestData.GenerateSale();

        // Act
        var result = sale.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that RecalculateTotal correctly sums all item totals.
    /// </summary>
    [Fact(DisplayName = "RecalculateTotal should sum all item totals correctly")]
    public void Given_SaleWithItems_When_RecalculateTotal_Then_TotalAmountShouldBeCorrect()
    {
        // Arrange
        var sale = SaleTestData.GenerateSale();

        var expectedTotal = sale.Items.Sum(i => i.Total);

        // Act
        sale.RecalculateTotal();

        // Assert
        Assert.Equal(expectedTotal, sale.TotalAmount);
    }
}
