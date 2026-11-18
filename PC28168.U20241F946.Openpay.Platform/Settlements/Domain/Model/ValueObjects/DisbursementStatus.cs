namespace PC28168.U20241F946.Openpay.Platform.Settlements.Domain.Model.ValueObjects;

/// <summary>
///     Possible states of a disbursement order.
/// </summary>
/// <remarks>
///     Author: Sebastian Lopez
/// </remarks>
public enum DisbursementStatus
{
    /// <summary>
    ///     The order is waiting to be processed. Default status for a new order.
    /// </summary>
    Queued = 10,

    /// <summary>
    ///     The order was processed.
    /// </summary>
    Processed = 11,

    /// <summary>
    ///     The order was cancelled.
    /// </summary>
    Cancelled = 12,

    /// <summary>
    ///     The order failed.
    /// </summary>
    Failed = 13
}
