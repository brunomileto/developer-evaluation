using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ISaleRepository using Entity Framework Core
/// </summary>
public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of SaleRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _context.Sales.AddAsync(sale, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    /// <inheritdoc />
    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        var existingSale = await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == sale.Id, cancellationToken);

        if (existingSale is null)
            throw new KeyNotFoundException($"Sale with ID {sale.Id} not found");

        existingSale.CustomerName = sale.CustomerName;
        existingSale.BranchName = sale.BranchName;
        existingSale.Status = sale.Status;
        existingSale.RecalculateTotal();
        
        var itemsToRemove = _context.SaleItems.Where(x => x.SaleId == existingSale.Id);
        _context.SaleItems.RemoveRange(itemsToRemove);
        
        var newItems = sale.Items
            .Select(item => SaleItem.Create(
                item.ProductId,
                item.ProductName,
                item.UnitPrice,
                item.Quantity,
                existingSale.Id
            ))
            .ToList();

        existingSale.Items = newItems;

        await _context.SaveChangesAsync(cancellationToken);
        return existingSale;
    }

    /// <inheritdoc />
    public async Task<(IEnumerable<Sale> Items, int TotalCount)> GetPagedRawAsync(
        int page,
        int size,
        string? order,
        string? customerName,
        string? branchName,
        string? status,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Sale> query = _context.Sales.AsNoTracking();

        // Filter by customer name (supports wildcard *)
        if (!string.IsNullOrWhiteSpace(customerName))
        {
            var pattern = customerName.Replace("*", "%");
            query = query.Where(s => EF.Functions.ILike(s.CustomerName, pattern));
        }

        // Filter by branch name (supports wildcard *)
        if (!string.IsNullOrWhiteSpace(branchName))
        {
            var pattern = branchName.Replace("*", "%");
            query = query.Where(s => EF.Functions.ILike(s.BranchName, pattern));
        }

        // Filter by status
        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(s => s.Status.ToString().Equals(status, StringComparison.OrdinalIgnoreCase));
        }

        // Ordering support (e.g. "saleDate desc, customerName")
        if (!string.IsNullOrWhiteSpace(order))
        {
            try
            {
                query = query.OrderBy(order);
            }
            catch (ParseException)
            {
                // Fallback to default ordering if parsing fails
                query = query.OrderBy(s => s.SaleDate);
            }
        }
        else
        {
            query = query.OrderBy(s => s.SaleDate);
        }

        // Get total count
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination
        var items = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
    
    /// <inheritdoc />
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sale = await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (sale is null)
            return false;

        _context.Sales.Remove(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

}
