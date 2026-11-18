using PC28168.U20241F946.Openpay.Platform.Settlements.Domain.Model.ValueObjects;
using PC28168.U20241F946.Openpay.Platform.Shared.Domain.Model.ValueObjects;

namespace PC28168.U20241F946.Openpay.Platform.Settlements.Domain.Model.Commands;

/// <summary>
///     Command to create a disbursement order.
/// </summary>
/// <remarks>
///     Author: Sebastian Lopez
/// </remarks>
/// <param name="OrderTrackingCode">The Openpay reference that identifies the order.</param>
/// <param name="AffiliateStoreId">The identifier of the affiliate store.</param>
/// <param name="ExecutionSchedule">The interval in which the disbursement is scheduled.</param>
/// <param name="TransferCap">The maximum amount to transfer.</param>
/// <param name="Status">The status of the order. When null, <see cref="DisbursementStatus.Queued"/> is used.</param>
/// <param name="User">The identifier of the operator user that registers the order.</param>
/// <param name="Remarks">Optional remarks about the order.</param>
public record CreateDisbursementOrderCommand(
    OpenpayReference OrderTrackingCode,
    StoreIdentifier AffiliateStoreId,
    DateInterval ExecutionSchedule,
    FinancialAmount TransferCap,
    DisbursementStatus? Status,
    string User,
    string? Remarks);
