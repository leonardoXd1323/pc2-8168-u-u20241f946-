using PC28168.U20241F946.Openpay.Platform.Shared.Domain.Model;

namespace PC28168.U20241F946.Openpay.Platform.Settlements.Domain.Model.Aggregates;

/// <summary>
///     Audit extension for the <see cref="DisbursementOrder"/> aggregate.
/// </summary>
/// <remarks>
///     CreatedAt and UpdatedAt are automatically managed by the persistence layer through the
///     AuditableEntityInterceptor of the Shared bounded context. Author: Sebastian Lopez
/// </remarks>
public partial class DisbursementOrder : IAuditableEntity
{
    /// <summary>
    ///     Gets the timestamp when this order was created. Maps to column created_at.
    /// </summary>
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    ///     Gets the timestamp when this order was last updated. Maps to column updated_at.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }
}
