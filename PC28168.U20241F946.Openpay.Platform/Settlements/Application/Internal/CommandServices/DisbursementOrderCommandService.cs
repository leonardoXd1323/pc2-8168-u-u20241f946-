using PC28168.U20241F946.Openpay.Platform.Settlements.Application.Errors;
using PC28168.U20241F946.Openpay.Platform.Settlements.Application.Services;
using PC28168.U20241F946.Openpay.Platform.Settlements.Domain.Model.Aggregates;
using PC28168.U20241F946.Openpay.Platform.Settlements.Domain.Model.Commands;
using PC28168.U20241F946.Openpay.Platform.Settlements.Domain.Repositories;
using PC28168.U20241F946.Openpay.Platform.Shared.Application.Patterns;
using PC28168.U20241F946.Openpay.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace PC28168.U20241F946.Openpay.Platform.Settlements.Application.Internal.CommandServices;

/// <summary>
///     Application service that handles the commands of the disbursement order aggregate.
/// </summary>
/// <remarks>
///     Duplicate order tracking codes are detected at application level and at database level (unique index).
///     Author: Sebastian Lopez
/// </remarks>
/// <param name="disbursementOrderRepository">Repository for accessing disbursement order data.</param>
/// <param name="unitOfWork">Unit of work for managing the transaction scope.</param>
/// <param name="logger">Logger for diagnostic and error reporting.</param>
public class DisbursementOrderCommandService(
    IDisbursementOrderRepository disbursementOrderRepository,
    IUnitOfWork unitOfWork,
    ILogger<DisbursementOrderCommandService> logger)
    : IDisbursementOrderCommandService
{
    /// <inheritdoc />
    public async Task<Result<DisbursementOrder, CreateDisbursementOrderError>> Handle(
        CreateDisbursementOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        // Business rule validation happens here; an ArgumentException is propagated to the interface layer.
        var disbursementOrder = new DisbursementOrder(command);

        var existingOrder = await disbursementOrderRepository
            .FindByOrderTrackingCodeAsync(command.OrderTrackingCode, cancellationToken);
        if (existingOrder is not null)
        {
            logger.LogWarning("Duplicate disbursement order rejected for OrderTrackingCode {OrderTrackingCode}",
                command.OrderTrackingCode);
            return new Result<DisbursementOrder, CreateDisbursementOrderError>.Failure(
                CreateDisbursementOrderError.DuplicateOrderTrackingCode);
        }

        try
        {
            await disbursementOrderRepository.AddAsync(disbursementOrder, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<DisbursementOrder, CreateDisbursementOrderError>.Success(disbursementOrder);
        }
        catch (DbUpdateException ex) when (IsDuplicateKeyViolation(ex))
        {
            logger.LogWarning(ex, "Duplicate key violation creating disbursement order for OrderTrackingCode {OrderTrackingCode}",
                command.OrderTrackingCode);
            return new Result<DisbursementOrder, CreateDisbursementOrderError>.Failure(
                CreateDisbursementOrderError.DuplicateOrderTrackingCode);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed creating disbursement order for OrderTrackingCode {OrderTrackingCode}",
                command.OrderTrackingCode);
            return new Result<DisbursementOrder, CreateDisbursementOrderError>.Failure(
                CreateDisbursementOrderError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating disbursement order for OrderTrackingCode {OrderTrackingCode}",
                command.OrderTrackingCode);
            return new Result<DisbursementOrder, CreateDisbursementOrderError>.Failure(
                CreateDisbursementOrderError.UnexpectedError);
        }
    }

    /// <summary>
    ///     Determines whether a <see cref="DbUpdateException"/> represents a duplicate key constraint violation.
    /// </summary>
    /// <param name="exception">The exception to inspect.</param>
    /// <returns><c>true</c> if the exception is due to a MySQL duplicate key error (code 1062); otherwise <c>false</c>.</returns>
    private static bool IsDuplicateKeyViolation(DbUpdateException exception)
    {
        for (Exception? current = exception; current is not null; current = current.InnerException)
        {
            if (!string.Equals(current.GetType().Name, "MySqlException", StringComparison.Ordinal)) continue;
            var numberProperty = current.GetType().GetProperty("Number");
            if (numberProperty?.PropertyType == typeof(int) &&
                numberProperty.GetValue(current) is int errorCode &&
                errorCode == 1062)
                return true;
        }

        return false;
    }
}
