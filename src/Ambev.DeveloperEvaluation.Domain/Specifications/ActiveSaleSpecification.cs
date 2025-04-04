using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Specifications;

namespace Ambev.DeveloperEvaluation.Domain.Specifications;

/// <summary>
/// Specification to determine if a sale is currently active.
/// </summary>
/// <remarks>
/// This is used to ensure that operations are only performed on active sales.
/// </remarks>
public class ActiveSaleSpecification : ISpecification<Sale>
{
    /// <summary>
    /// Checks whether the given sale is active.
    /// </summary>
    /// <param name="sale">The sale entity to check</param>
    /// <returns>True if the sale has an active status, false otherwise</returns>
    public bool IsSatisfiedBy(Sale sale)
        => sale.Status == Status.Active;
}