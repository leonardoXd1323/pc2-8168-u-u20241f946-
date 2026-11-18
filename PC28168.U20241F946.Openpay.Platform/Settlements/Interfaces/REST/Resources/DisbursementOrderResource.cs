namespace PC28168.U20241F946.Openpay.Platform.Settlements.Interfaces.REST.Resources;

/// <summary>
///     Represents the data provided by the server about a disbursement order.
/// </summary>
/// <remarks>
///     Author: Sebastian Lopez
/// </remarks>
/// <param name="Id">The server-generated identifier of the order.</param>
/// <param name="OrderTrackingCode">The Openpay reference that identifies the order.</param>
/// <param name="AffiliateStoreId">The identifier of the affiliate store.</param>
/// <param name="ScheduledFrom">The first scheduled date, formatted as yyyy-MM-dd.</param>
/// <param name="ScheduledTo">The last scheduled date, formatted as yyyy-MM-dd.</param>
/// <param name="TransferCapAmount">The maximum amount to transfer.</param>
/// <param name="TransferCapCurrencyCode">The currency code of the transfer cap.</param>
/// <param name="Status">The status of the order.</param>
/// <param name="User">The operator user that registered the order.</param>
/// <param name="Remarks">The optional remarks.</param>
/// <param name="UpdatedAt">The date and time of the last update of the record.</param>
public record DisbursementOrderResource(
    int Id,
    string OrderTrackingCode,
    string AffiliateStoreId,
    string ScheduledFrom,
    string ScheduledTo,
    decimal TransferCapAmount,
    string TransferCapCurrencyCode,
    string Status,
    string User,
    string? Remarks,
    DateTimeOffset? UpdatedAt);
