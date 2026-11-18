using PC28168.U20241F946.Openpay.Platform.Settlements.Domain.Model.Aggregates;
using PC28168.U20241F946.Openpay.Platform.Settlements.Domain.Model.ValueObjects;
using PC28168.U20241F946.Openpay.Platform.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using PC28168.U20241F946.Openpay.Platform.Shared.Infrastructure.Persistence.EFC.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace PC28168.U20241F946.Openpay.Platform.Shared.Infrastructure.Persistence.EFC.Configuration;

/// <summary>
///     Application database context. Maps the domain concepts to the relational schema using the Fluent API.
/// </summary>
/// <remarks>
///     Author: Sebastian Lopez
/// </remarks>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        // Apply audit timestamp interceptor for all IAuditableEntity implementations
        builder.AddInterceptors(new AuditableEntityInterceptor());
        base.OnConfiguring(builder);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Settlements Bounded Context
        builder.Entity<DisbursementOrder>(entity =>
        {
            entity.HasKey(d => d.Id).HasName("pk_disbursement_orders");
            entity.Property(d => d.Id).IsRequired().ValueGeneratedOnAdd();

            entity.Property(d => d.OrderTrackingCode)
                .HasConversion(valueObject => valueObject.Value, value => new OpenpayReference(value))
                .IsRequired();
            entity.HasIndex(d => d.OrderTrackingCode)
                .IsUnique()
                .HasDatabaseName("ux_disbursement_orders_order_tracking_code");

            entity.Property(d => d.AffiliateStoreId)
                .HasConversion(valueObject => valueObject.Value, value => new StoreIdentifier(value))
                .IsRequired();

            entity.OwnsOne(d => d.ExecutionSchedule, schedule =>
            {
                schedule.Property(s => s.ScheduledFrom).HasColumnName("scheduled_from").IsRequired();
                schedule.Property(s => s.ScheduledTo).HasColumnName("scheduled_to").IsRequired();
            });
            entity.Navigation(d => d.ExecutionSchedule).IsRequired();

            entity.OwnsOne(d => d.TransferCap, cap =>
            {
                cap.Property(c => c.Amount)
                    .HasColumnName("transfer_cap_amount")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
                cap.Property(c => c.CurrencyCode)
                    .HasColumnName("transfer_cap_currency_code")
                    .HasMaxLength(10)
                    .IsRequired();
            });
            entity.Navigation(d => d.TransferCap).IsRequired();

            entity.Property(d => d.Status).IsRequired();
            entity.Property(d => d.User).IsRequired().HasMaxLength(DisbursementOrder.UserMaxLength);
            entity.Property(d => d.Remarks).HasMaxLength(DisbursementOrder.RemarksMaxLength);
        });

        builder.UseSnakeCaseNamingConvention();
    }
}
