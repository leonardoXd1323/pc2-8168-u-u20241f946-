using PC28168.U20241F946.Openpay.Platform.Settlements.Application.Errors;
using PC28168.U20241F946.Openpay.Platform.Settlements.Domain.Model.Aggregates;
using PC28168.U20241F946.Openpay.Platform.Settlements.Domain.Model.Commands;
using PC28168.U20241F946.Openpay.Platform.Shared.Application.Patterns;

namespace PC28168.U20241F946.Openpay.Platform.Settlements.Application.Services;

/// <summary>
///     Command service interface for disbursement order operations.
/// </summary>
/// <remarks>
///     Author: Sebastian Lopez
/// </remarks>
public interface IDisbursementOrderCommandService
{
    /// <summary>
    ///     Handles the create disbursement order command.
    /// </summary>
    /// <remarks>
    ///     Rejects the command when an order with the same order tracking code already exists; otherwise creates and
    ///     persists the order.
    /// </remarks>
    /// <param name="command">The <see cref="CreateDisbursementOrderCommand"/> command.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the created order on success, or the error reason on failure.</returns>
    /// <exception cref="ArgumentException">Thrown when the command contains values that break a business rule.</exception>
    Task<Result<DisbursementOrder, CreateDisbursementOrderError>> Handle(CreateDisbursementOrderCommand command,
        CancellationToken cancellationToken = default);
}
