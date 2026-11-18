using System.Net.Mime;
using PC28168.U20241F946.Openpay.Platform.Resources;
using PC28168.U20241F946.Openpay.Platform.Settlements.Application.Services;
using PC28168.U20241F946.Openpay.Platform.Settlements.Interfaces.REST.Resources;
using PC28168.U20241F946.Openpay.Platform.Settlements.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace PC28168.U20241F946.Openpay.Platform.Settlements.Interfaces.REST;

/// <summary>
///     Disbursement orders' controller. Route: /api/v1/disbursement-orders.
/// </summary>
/// <remarks>
///     Author: Sebastian Lopez
/// </remarks>
/// <param name="disbursementOrderCommandService">The disbursement order command service.</param>
/// <param name="localizer">String localizer for response messages.</param>
/// <param name="logger">Logger for diagnostics and error reporting.</param>
[ApiController]
[Route("api/v1/[controller]")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Disbursement Orders")]
public class DisbursementOrdersController(
    IDisbursementOrderCommandService disbursementOrderCommandService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<DisbursementOrdersController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Creates a disbursement order.
    /// </summary>
    /// <param name="resource">The <see cref="CreateDisbursementOrderResource"/> resource.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     201 with the created disbursement order; 400 when a value is invalid; 409 when the order tracking code
    ///     already exists; 500 for unexpected errors.
    /// </returns>
    [HttpPost]
    [EndpointSummary("Create a disbursement order")]
    [EndpointDescription(
        "Registers a new disbursement order for an affiliate store. The order tracking code must be unique. " +
        "The status defaults to Queued when it is not provided. Error messages follow the Accept-Language header (en, en-US, es, es-PE).")]
    [ProducesResponseType(typeof(DisbursementOrderResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateDisbursementOrder([FromBody] CreateDisbursementOrderResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = CreateDisbursementOrderCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await disbursementOrderCommandService.Handle(command, cancellationToken);
            return ActionResultFromCreateDisbursementOrderResultAssembler
                .ToActionResultFromCreateDisbursementOrderResult(result, this, localizer);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while creating disbursement order {OrderTrackingCode}",
                resource.OrderTrackingCode);
            return Problem(
                title: localizer["InvalidModelStateTitle"].Value,
                detail: ValidationErrorMessageAssembler.ToMessageFromException(ex, localizer),
                statusCode: StatusCodes.Status400BadRequest);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating disbursement order {OrderTrackingCode}",
                resource.OrderTrackingCode);
            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorCreatingDisbursementOrder"].Value,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
