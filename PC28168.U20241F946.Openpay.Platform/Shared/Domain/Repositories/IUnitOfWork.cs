namespace PC28168.U20241F946.Openpay.Platform.Shared.Domain.Repositories;

/// <summary>
///     Unit of work interface
/// </summary>
/// <remarks>
///     This interface defines the basic operations for a unit of work
///     Author: Sebastian Lopez
/// </remarks>
public interface IUnitOfWork
{
    /// <summary>
    ///     Commit changes to the database
    /// </summary>
    Task CompleteAsync(CancellationToken cancellationToken = default);
}