namespace PC28168.U20241F946.Openpay.Platform.Settlements.Application.Errors;

/// <summary>
///     Represents the errors that can occur when creating a disbursement order.
/// </summary>
/// <remarks>
///     Author: Sebastian Lopez
/// </remarks>
public enum CreateDisbursementOrderError
{
    /// <summary>
    ///     A disbursement order with the same order tracking code already exists.
    /// </summary>
    DuplicateOrderTrackingCode,

    /// <summary>
    ///     An unexpected error occurred during the operation.
    /// </summary>
    UnexpectedError
}
