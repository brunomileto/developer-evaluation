using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Specifications;

namespace Ambev.DeveloperEvaluation.Domain.Specifications;

/// <summary>
/// Specification to validate that a sale item has a valid quantity.
/// </summary>
/// <remarks>
/// This specification enforces a business rule that sale item quantities
/// must be greater than 0 and no more than 20 units.
/// </remarks>
public class ValidQuantitySpecification : ISpecification<SaleItem>
{
    /// <summary>
    /// Checks whether the given sale item satisfies the valid quantity rule.
    /// </summary>
    /// <param name="item">The sale item to validate</param>
    /// <returns>True if quantity is within range, false otherwise</returns>
    public bool IsSatisfiedBy(SaleItem item)
        => item.Quantity is > 0 and <= 20;
}