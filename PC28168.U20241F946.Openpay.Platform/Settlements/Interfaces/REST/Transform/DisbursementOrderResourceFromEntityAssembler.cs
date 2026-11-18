using System.Globalization;
using PC28168.U20241F946.Openpay.Platform.Settlements.Domain.Model.Aggregates;
using PC28168.U20241F946.Openpay.Platform.Settlements.Interfaces.REST.Resources;

namespace PC28168.U20241F946.Openpay.Platform.Settlements.Interfaces.REST.Transform;

/// <summary>
///     Assembles a <see cref="DisbursementOrderResource"/> from a <see cref="DisbursementOrder"/>.
/// </summary>
/// <remarks>
///     Author: Sebastian Lopez
/// </remarks>
public static class DisbursementOrderResourceFromEntityAssembler
{
    /// <summary>
    ///     Assembles a <see cref="DisbursementOrderResource"/> from a <see cref="DisbursementOrder"/>.
    /// </summary>
    /// <param name="entity">The <see cref="DisbursementOrder"/> entity.</param>
    /// <returns>A <see cref="DisbursementOrderResource"/> assembled from the entity.</returns>
    public static DisbursementOrderResource ToResourceFromEntity(DisbursementOrder entity) =>
        new(
            entity.Id,
            entity.OrderTrackingCode.Value.ToString(),
            entity.AffiliateStoreId.Value.ToString(),
            entity.ExecutionSchedule.ScheduledFrom.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            entity.ExecutionSchedule.ScheduledTo.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            entity.TransferCap.Amount,
            entity.TransferCap.CurrencyCode,
            entity.Status.ToString(),
            entity.User,
            entity.Remarks,
            entity.UpdatedAt);
}
