using PC28168.U20241F946.Openpay.Platform.Resources;
using PC28168.U20241F946.Openpay.Platform.Settlements.Application.Errors;
using PC28168.U20241F946.Openpay.Platform.Settlements.Domain.Model.Aggregates;
using PC28168.U20241F946.Openpay.Platform.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace PC28168.U20241F946.Openpay.Platform.Settlements.Interfaces.REST.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of handling the create disbursement order command.
/// </summary>
/// <remarks>
///     Author: Sebastian Lopez
/// </remarks>
public static class ActionResultFromCreateDisbursementOrderResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult"/> from the result of a create disbursement order operation.
    /// </summary>
    /// <param name="result">The application result produced after handling the command.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromCreateDisbursementOrderResult(
        Result<DisbursementOrder, CreateDisbursementOrderError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<DisbursementOrder, CreateDisbursementOrderError>.Success success =>
                controller.StatusCode(StatusCodes.Status201Created,
                    DisbursementOrderResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<DisbursementOrder, CreateDisbursementOrderError>.Failure failure =>
                failure.Error switch
                {
                    CreateDisbursementOrderError.DuplicateOrderTrackingCode =>
                        controller.Problem(
                            title: localizer["DisbursementOrderDuplicatedTitle"].Value,
                            detail: localizer["DisbursementOrderDuplicated"].Value,
                            statusCode: StatusCodes.Status409Conflict),

                    _ => controller.Problem(
                        title: localizer["UnexpectedServerError"].Value,
                        detail: localizer["UnexpectedErrorCreatingDisbursementOrder"].Value,
                        statusCode: StatusCodes.Status500InternalServerError)
                },

            _ => controller.Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                statusCode: StatusCodes.Status500InternalServerError)
        };
}
