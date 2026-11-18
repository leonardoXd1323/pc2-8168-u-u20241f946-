using System.ComponentModel.DataAnnotations;

namespace PC28168.U20241F946.Openpay.Platform.Settlements.Interfaces.REST.Resources;

/// <summary>
///     Represents the data required to create a disbursement order.
/// </summary>
/// <remarks>
///     Value objects are specified as separated primitive values. Author: Sebastian Lopez
/// </remarks>
/// <param name="OrderTrackingCode">The Openpay reference (GUID) that identifies the order (required).</param>
/// <param name="AffiliateStoreId">The identifier (GUID) of the affiliate store (required).</param>
/// <param name="ScheduledFrom">The first scheduled date, formatted as yyyy-MM-dd (required).</param>
/// <param name="ScheduledTo">The last scheduled date, formatted as yyyy-MM-dd (required).</param>
/// <param name="TransferCapAmount">The maximum amount to transfer (required).</param>
/// <param name="TransferCapCurrencyCode">The currency code of the transfer cap (required).</param>
/// <param name="Status">The status: Queued, Processed, Cancelled or Failed. Defaults to Queued (optional).</param>
/// <param name="User">The operator user that registers the order, up to 20 characters (required).</param>
/// <param name="Remarks">Optional remarks, up to 85 characters.</param>
public record CreateDisbursementOrderResource(
    [Required(ErrorMessage = "RequiredOrderTrackingCode")]
    string OrderTrackingCode,
    [Required(ErrorMessage = "RequiredAffiliateStoreId")]
    string AffiliateStoreId,
    [Required(ErrorMessage = "RequiredScheduledFrom")]
    string ScheduledFrom,
    [Required(ErrorMessage = "RequiredScheduledTo")]
    string ScheduledTo,
    [Required(ErrorMessage = "RequiredTransferCapAmount")]
    decimal? TransferCapAmount,
    [Required(ErrorMessage = "RequiredTransferCapCurrencyCode")]
    string TransferCapCurrencyCode,
    string? Status,
    [Required(ErrorMessage = "RequiredUser")]
    string User,
    string? Remarks);
