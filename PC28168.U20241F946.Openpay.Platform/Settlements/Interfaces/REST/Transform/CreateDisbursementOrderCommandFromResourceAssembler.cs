using System.Globalization;
using PC28168.U20241F946.Openpay.Platform.Settlements.Domain.Model.Commands;
using PC28168.U20241F946.Openpay.Platform.Settlements.Domain.Model.ValueObjects;
using PC28168.U20241F946.Openpay.Platform.Settlements.Interfaces.REST.Resources;
using PC28168.U20241F946.Openpay.Platform.Shared.Domain.Model.ValueObjects;

namespace PC28168.U20241F946.Openpay.Platform.Settlements.Interfaces.REST.Transform;

/// <summary>
///     Assembles a <see cref="CreateDisbursementOrderCommand"/> from a <see cref="CreateDisbursementOrderResource"/>.
/// </summary>
/// <remarks>
///     Author: Sebastian Lopez
/// </remarks>
public static class CreateDisbursementOrderCommandFromResourceAssembler
{
    private const string DateFormat = "yyyy-MM-dd";

    /// <summary>
    ///     Assembles a <see cref="CreateDisbursementOrderCommand"/> from a <see cref="CreateDisbursementOrderResource"/>.
    /// </summary>
    /// <param name="resource">The <see cref="CreateDisbursementOrderResource"/> resource.</param>
    /// <returns>A <see cref="CreateDisbursementOrderCommand"/> assembled from the resource.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when a value cannot be converted to its domain type or breaks a value object rule.
    /// </exception>
    public static CreateDisbursementOrderCommand ToCommandFromResource(CreateDisbursementOrderResource resource)
    {
        var orderTrackingCode = new OpenpayReference(
            ParseGuid(resource.OrderTrackingCode, nameof(resource.OrderTrackingCode)));
        var affiliateStoreId = new StoreIdentifier(
            ParseGuid(resource.AffiliateStoreId, nameof(resource.AffiliateStoreId)));
        var executionSchedule = new DateInterval(
            ParseDate(resource.ScheduledFrom, nameof(resource.ScheduledFrom)),
            ParseDate(resource.ScheduledTo, nameof(resource.ScheduledTo)));
        var transferCap = new FinancialAmount(
            resource.TransferCapAmount ??
            throw new ArgumentException("TransferCapAmount is required.", nameof(resource.TransferCapAmount)),
            resource.TransferCapCurrencyCode);

        return new CreateDisbursementOrderCommand(
            orderTrackingCode,
            affiliateStoreId,
            executionSchedule,
            transferCap,
            ParseStatus(resource.Status),
            resource.User,
            resource.Remarks);
    }

    private static Guid ParseGuid(string value, string parameterName) =>
        Guid.TryParse(value, out var guid)
            ? guid
            : throw new ArgumentException($"{parameterName} must be a valid GUID.", parameterName);

    private static DateOnly ParseDate(string value, string parameterName) =>
        DateOnly.TryParseExact(value, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
            ? date
            : throw new ArgumentException($"{parameterName} must be a valid date ({DateFormat}).", parameterName);

    private static DisbursementStatus? ParseStatus(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        return Enum.TryParse<DisbursementStatus>(value, true, out var status) && Enum.IsDefined(status)
            ? status
            : throw new ArgumentException("Status is not a valid disbursement status.", "Status");
    }
}
