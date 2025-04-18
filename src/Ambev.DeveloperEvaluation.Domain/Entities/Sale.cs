using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Sales.Events;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale in the system, including customer and itemized product information.
/// </summary>
public class Sale : BaseEntity
{
    
    /// <summary>
    /// Unique number that identifies the sale (e.g., invoice number).
    /// </summary>
    public string SaleNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Date and time when the sale was made.
    /// </summary>
    public DateTime SaleDate { get; private set; }

    /// <summary>
    /// External identity: customer ID.
    /// </summary>
    public Guid CustomerId { get; private set; }

    /// <summary>
    /// Denormalized: customer full name at the moment of the sale.
    /// </summary>
    public string CustomerName { get; private set; } = string.Empty;

    /// <summary>
    /// External identity: branch ID where the sale occurred.
    /// </summary>
    public Guid BranchId { get; private set; }

    /// <summary>
    /// Denormalized: branch name.
    /// </summary>
    public string BranchName { get; private set; } = string.Empty;

    /// <summary>
    /// Total value of the sale (sum of all item totals).
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Gets the sale's current status.
    /// Indicates whether the sale is active, or cancelled in the system.
    /// </summary>
    public Status Status { get; private set; } = Status.Active;

    /// <summary>
    /// List of items that compose the sale.
    /// </summary>
    public List<SaleItem> Items { get; private set; } = [];
    
    /// <summary>
    /// Applies discount rules and calculates total sale amount.
    /// </summary>
    public void RecalculateTotal()
    {
        foreach (var item in Items)
        {
            item.CalculateTotal();
        }

        TotalAmount = Items.Sum(i => i.Total);
    }
    
    /// <summary>
    /// Checks whether the sale is currently active.
    /// </summary>
    /// <returns>
    /// True if the sale's <see cref="Status"/> is <c>Status.Active</c>; otherwise, false.
    /// </returns>
    public bool IsActive()
    {
        var spec = new ActiveSaleSpecification();
        return spec.IsSatisfiedBy(this);
    }


    
    /// <summary>
    /// Validates the sale using the SaleValidator.
    /// </summary>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(e => (ValidationErrorDetail)e)
        };
    }
    
    /// <summary>
    /// Factory method to create a new <see cref="Sale"/> instance with all required data and automatic total calculation.
    /// </summary>
    /// <param name="customerId">The unique identifier of the customer making the purchase.</param>
    /// <param name="customerName">The full name of the customer at the time of the sale (denormalized).</param>
    /// <param name="branchId">The unique identifier of the branch where the sale occurred.</param>
    /// <param name="branchName">The name of the branch at the time of the sale (denormalized).</param>
    /// <param name="saleNumber">The unique number that identifies the sale.</param>
    /// <param name="items">The list of <see cref="SaleItem"/> instances that make up the sale.</param>
    /// <returns>
    /// A new <see cref="Sale"/> instance with totals calculated and all business rules applied.
    /// </returns>
    public static Sale Create(Guid customerId, string customerName, Guid branchId, string branchName, string saleNumber, List<SaleItem> items)
    {
        var sale = new Sale
        {
            CustomerId = customerId,
            CustomerName = customerName,
            BranchId = branchId,
            BranchName = branchName,
            Items = items,
            Status = Status.Active,
            SaleNumber = saleNumber
        };
        
        sale.RecalculateTotal();
        sale.AddDomainEvent(new SaleCreatedDomainEvent(sale.Id, sale.CustomerId, sale.SaleDate));
        return sale;
    }
    
    public void Cancel()
    {
        if (Status == Status.Cancelled)
            return;

        Status = Status.Cancelled;
    }
    
    

    public void Update(string customerName, string branchName, Status status, List<SaleItem> items)
    {
        var hasChanges = false;

        if (CustomerName != customerName)
        {
            CustomerName = customerName;
            hasChanges = true;
        }

        if (!Status.Equals(status))
        {
            Status = status;
            hasChanges = true;
        }

        if (BranchName != branchName)
        {
            BranchName = branchName;
            hasChanges = true;
        }

        if (HasItemListChanged(Items, items))
        {
            Items = items;
            RecalculateTotal();
            hasChanges = true;
        }

        if (hasChanges)
        {
            AddDomainEvent(new SaleModifiedDomainEvent(Id, CustomerId, DateTime.UtcNow));
        }
    }

    private bool HasItemListChanged(List<SaleItem> current, List<SaleItem> updated)
    {
        if (current.Count != updated.Count) return true;

        foreach (var currentItem in current)
        {
            var matching = updated.FirstOrDefault(u => u.Id == currentItem.Id);
            if (matching == null || currentItem.HasMeaningfulChanges(matching))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Private constructor used to initialize default values when creating a new <see cref="Sale"/> instance.
    /// Sets the <see cref="SaleDate"/> to the current UTC date and time.
    /// </summary>
    private Sale()
    {
        SaleDate = DateTime.UtcNow;
    }

}