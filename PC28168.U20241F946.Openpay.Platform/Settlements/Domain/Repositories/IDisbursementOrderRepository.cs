using PC28168.U20241F946.Openpay.Platform.Settlements.Domain.Model.Aggregates;
using PC28168.U20241F946.Openpay.Platform.Settlements.Domain.Model.ValueObjects;
using PC28168.U20241F946.Openpay.Platform.Shared.Domain.Repositories;

namespace PC28168.U20241F946.Openpay.Platform.Settlements.Domain.Repositories;

/// <summary>
///     The disbursement order repository contract.
/// </summary>
/// <remarks>
///     Author: Sebastian Lopez
/// </remarks>
public interface IDisbursementOrderRepository : IBaseRepository<DisbursementOrder>
{
    /// <summary>
    ///     Finds a disbursement order by its order tracking code.
    /// </summary>
    /// <param name="orderTrackingCode">The Openpay reference to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The disbursement order if found, or null otherwise.</returns>
    Task<DisbursementOrder?> FindByOrderTrackingCodeAsync(OpenpayReference orderTrackingCode,
        CancellationToken cancellationToken = default);
}
