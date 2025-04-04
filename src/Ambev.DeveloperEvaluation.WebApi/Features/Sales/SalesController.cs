using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales
{
    /// <summary>
    /// Controller for managing sales operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController(IMediator mediator, IMapper mapper) : BaseController
    {
        /// <summary>
        /// Creates a new sale
        /// </summary>
        /// <param name="request">The sale creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>HTTP response with created sale details</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = mapper.Map<CreateSaleCommand>(request);
            var result = await mediator.Send(command, cancellationToken);

            var response = mapper.Map<CreateSaleResponse>(result);
            return Created(string.Empty, null, response); 
        }

        /// <summary>
        /// Retrieves a sale by its ID
        /// </summary>
        /// <param name="id">The unique identifier of the sale</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The sale details if found</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSale([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var request = new GetSaleRequest { Id = id };
            var validator = new GetSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(f => (ValidationErrorDetail)f);
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Validation failed",
                    Errors = errors
                });
            }

            var command = mapper.Map<GetSaleCommand>(id);
            var result = await mediator.Send(command, cancellationToken);

            if (result is null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = $"The sale with ID {id} does not exist in our database",
                    Errors = []
                });
            }

            var response = mapper.Map<GetSaleResponse>(result);
            return Ok(new ApiResponseWithData<GetSaleResponse>
            {
                Success = true,
                Message = "Sale retrieved successfully",
                Data = response
            });
        }


        /// <summary>
        /// Retrieves a paginated list of sales with optional filtering and ordering.
        /// </summary>
        /// <param name="_page">Page number (default: 1)</param>
        /// <param name="_size">Page size (default: 10)</param>
        /// <param name="_order">Ordering fields (e.g. "saleDate desc, customerName")</param>
        /// <param name="customerName">Filter by customer name (supports partial match with *)</param>
        /// <param name="branchName">Filter by branch name (supports partial match with *)</param>
        /// <param name="status">Filter by status (exact match)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paginated list of sales</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResponse<GetSalesListItemResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSales(
            [FromQuery(Name = "_page")] int page = 1,
            [FromQuery(Name = "_size")] int size = 10,
            [FromQuery(Name = "_order")] string? order = null,
            [FromQuery] string? customerName = null,
            [FromQuery] string? branchName = null,
            [FromQuery] string? status = null,
            CancellationToken cancellationToken = default)
        {
            var query = new GetSalesQuery
            {
                Page = page,
                Size = size,
                Order = order,
                CustomerName = customerName,
                BranchName = branchName,
                Status = status
            };

            var result = await mediator.Send(query, cancellationToken);
            var mapped = mapper.Map<List<GetSalesListItemResponse>>(result.Items);

            return OkPaginated(new PaginatedList<GetSalesListItemResponse>(
                mapped, result.TotalCount, result.CurrentPage, result.Items.Count()
            ));
        }
        
        /// <summary>
        /// Updates an existing sale by ID
        /// </summary>
        /// <param name="id">The ID of the sale to update</param>
        /// <param name="request">The updated sale data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated sale information</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<UpdateSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSale([FromRoute] Guid id, [FromBody] UpdateSaleRequest request, CancellationToken cancellationToken)
        {
            var validator = new UpdateSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = mapper.Map<UpdateSaleCommand>(request) with { Id = id };

            try
            {
                var result = await mediator.Send(command, cancellationToken);
                var response = mapper.Map<UpdateSaleResponse>(result);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message,
                    Errors = []
                });
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.Select(failure => (ValidationErrorDetail)failure);

                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Validation failed",
                    Errors = errors
                });
            }
        }
        
        /// <summary>
        /// Deletes an existing sale by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the sale to be deleted.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content if the deletion is successful; otherwise, an appropriate error response.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSale([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var validator = new DeleteSaleRequestValidator();
            var validation = await validator.ValidateAsync(new DeleteSaleRequest { Id = id }, cancellationToken);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var command = new DeleteSaleCommand(id);
            var result = await mediator.Send(command, cancellationToken);

            if (result is null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Sale not found"
                });
            }

            return NoContent();
        }

    }

}
