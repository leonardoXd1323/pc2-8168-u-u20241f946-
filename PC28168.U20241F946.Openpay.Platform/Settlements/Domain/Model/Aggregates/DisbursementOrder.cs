using PC28168.U20241F946.Openpay.Platform.Settlements.Domain.Model.Commands;
using PC28168.U20241F946.Openpay.Platform.Settlements.Domain.Model.ValueObjects;
using PC28168.U20241F946.Openpay.Platform.Shared.Domain.Model.ValueObjects;

namespace PC28168.U20241F946.Openpay.Platform.Settlements.Domain.Model.Aggregates;

/// <summary>
///     Represents an order to disburse funds to an affiliate store.
/// </summary>
/// <remarks>
///     This aggregate root enforces the business rules of a disbursement order. The order tracking code must be unique
///     within the system (checked by the application layer and by a unique index).
///     Audit timestamps are managed by the persistence layer. Author: Sebastian Lopez
/// </remarks>
public partial class DisbursementOrder
{
    /// <summary>
    ///     Maximum length allowed for <see cref="User"/>.
    /// </summary>
    public const int UserMaxLength = 20;

    /// <summary>
    ///     Maximum length allowed for <see cref="Remarks"/>.
    /// </summary>
    public const int RemarksMaxLength = 85;

    /// <summary>
    ///     Protected parameterless constructor for EF Core.
    /// </summary>
    protected DisbursementOrder()
    {
        OrderTrackingCode = null!;
        AffiliateStoreId = null!;
        ExecutionSchedule = null!;
        TransferCap = null!;
        User = string.Empty;
    }

    /// <summary>
    ///     Creates a new <see cref="DisbursementOrder"/> from a creation command.
    /// </summary>
    /// <param name="command">The <see cref="CreateDisbursementOrderCommand"/> command.</param>
    /// <exception cref="ArgumentNullException">Thrown when the command or the user is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the user is empty or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when the status is not defined, or when the user or remarks exceed the maximum length.
    /// </exception>
    public DisbursementOrder(CreateDisbursementOrderCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.User is null)
            throw new ArgumentNullException(nameof(User), "User cannot be null.");
        if (string.IsNullOrWhiteSpace(command.User))
            throw new ArgumentException("User cannot be empty or whitespace.", nameof(User));
        if (command.User.Length > UserMaxLength)
            throw new ArgumentOutOfRangeException(nameof(User), command.User.Length,
                $"User cannot be longer than {UserMaxLength} characters.");

        if (command.Remarks is { Length: > RemarksMaxLength })
            throw new ArgumentOutOfRangeException(nameof(Remarks), command.Remarks.Length,
                $"Remarks cannot be longer than {RemarksMaxLength} characters.");

        var status = command.Status ?? DisbursementStatus.Queued;
        if (!Enum.IsDefined(status))
            throw new ArgumentOutOfRangeException(nameof(Status), status, "Status is not a valid disbursement status.");

        OrderTrackingCode = command.OrderTrackingCode;
        AffiliateStoreId = command.AffiliateStoreId;
        ExecutionSchedule = command.ExecutionSchedule;
        TransferCap = command.TransferCap;
        Status = status;
        User = command.User;
        Remarks = string.IsNullOrWhiteSpace(command.Remarks) ? null : command.Remarks;
    }

    /// <summary>
    ///     Gets the server-generated identifier of the order.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    ///     Gets the Openpay reference that identifies the order.
    /// </summary>
    public OpenpayReference OrderTrackingCode { get; private set; }

    /// <summary>
    ///     Gets the identifier of the affiliate store.
    /// </summary>
    public StoreIdentifier AffiliateStoreId { get; private set; }

    /// <summary>
    ///     Gets the interval in which the disbursement is scheduled.
    /// </summary>
    public DateInterval ExecutionSchedule { get; private set; }

    /// <summary>
    ///     Gets the maximum amount to transfer.
    /// </summary>
    public FinancialAmount TransferCap { get; private set; }

    /// <summary>
    ///     Gets the status of the order.
    /// </summary>
    public DisbursementStatus Status { get; private set; }

    /// <summary>
    ///     Gets the identifier of the operator user that registered the order.
    /// </summary>
    public string User { get; private set; }

    /// <summary>
    ///     Gets the optional remarks about the order.
    /// </summary>
    public string? Remarks { get; private set; }
}
