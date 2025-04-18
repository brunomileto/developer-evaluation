using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Sale entity operations
/// </summary>
public interface ISaleRepository
{
    /// <summary>
    /// Creates a new sale in the repository.
    /// </summary>
    /// <param name="sale">The sale to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale</returns>
    Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a sale by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paged list of sales with optional filters and ordering.
    /// </summary>
    /// <param name="page">Page number</param>
    /// <param name="size">Page size</param>
    /// <param name="order">Ordering string (e.g., "saleDate desc")</param>
    /// <param name="customerName">Filter by customer name (supports *)</param>
    /// <param name="branchName">Filter by branch name (supports *)</param>
    /// <param name="status">Filter by sale status</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A tuple containing the paged result items and total count</returns>
    Task<(IEnumerable<Sale> Items, int TotalCount)> GetPagedRawAsync(
        int page,
        int size,
        string? order,
        string? customerName,
        string? branchName,
        string? status,
        CancellationToken cancellationToken = default
    );
    
    /// <summary>
    /// Updates an existing sale in the repository.
    /// </summary>
    /// <param name="sale">The sale to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale</returns>
    Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a sale from the repository.
    /// </summary>
    /// <param name="id">The unique identifier of the sale to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the sale was deleted, false if not found</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
