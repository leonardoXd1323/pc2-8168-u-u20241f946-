namespace PC28168.U20241F946.Openpay.Platform.Settlements.Domain.Model.ValueObjects;

/// <summary>
///     Value object representing the identifier (UUID) of an affiliate store.
/// </summary>
/// <remarks>
///     The identifier originates in another bounded context. Author: Sebastian Lopez
/// </remarks>
/// <param name="Value">The GUID value of the store identifier.</param>
public sealed record StoreIdentifier(Guid Value)
{
    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}
