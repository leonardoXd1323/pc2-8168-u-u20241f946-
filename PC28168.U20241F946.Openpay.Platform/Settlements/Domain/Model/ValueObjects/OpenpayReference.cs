namespace PC28168.U20241F946.Openpay.Platform.Settlements.Domain.Model.ValueObjects;

/// <summary>
///     Value object representing the Openpay reference that uniquely identifies an order.
/// </summary>
/// <remarks>
///     The identifier is generated in another bounded context. Author: Sebastian Lopez
/// </remarks>
/// <param name="Value">The GUID value of the reference.</param>
public sealed record OpenpayReference(Guid Value)
{
    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}
