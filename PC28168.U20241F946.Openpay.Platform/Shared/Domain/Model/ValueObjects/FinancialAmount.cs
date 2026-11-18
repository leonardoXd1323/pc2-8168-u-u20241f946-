namespace PC28168.U20241F946.Openpay.Platform.Shared.Domain.Model.ValueObjects;

/// <summary>
///     Value object representing a monetary amount expressed in a given currency.
/// </summary>
/// <remarks>
///     Author: Sebastian Lopez
/// </remarks>
public sealed record FinancialAmount
{
    /// <summary>
    ///     Initializes a new <see cref="FinancialAmount"/>.
    /// </summary>
    /// <param name="amount">The amount. Must be greater than or equal to zero.</param>
    /// <param name="currencyCode">The currency code. Must not be null, empty or whitespace.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="amount"/> is less than zero.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="currencyCode"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="currencyCode"/> is empty or whitespace.</exception>
    public FinancialAmount(decimal amount, string currencyCode)
    {
        if (amount < decimal.Zero)
            throw new ArgumentOutOfRangeException(nameof(Amount), amount,
                "Amount must be greater than or equal to zero.");

        if (currencyCode is null)
            throw new ArgumentNullException(nameof(CurrencyCode), "CurrencyCode cannot be null.");

        if (string.IsNullOrWhiteSpace(currencyCode))
            throw new ArgumentException("CurrencyCode cannot be empty or whitespace.", nameof(CurrencyCode));

        Amount = amount;
        CurrencyCode = currencyCode;
    }

    /// <summary>
    ///     Gets the amount.
    /// </summary>
    public decimal Amount { get; }

    /// <summary>
    ///     Gets the currency code.
    /// </summary>
    public string CurrencyCode { get; }

    /// <summary>
    ///     Multiplies the amount by the given factor, keeping the same currency code.
    /// </summary>
    /// <param name="factor">The multiplier factor.</param>
    /// <returns>A new <see cref="FinancialAmount"/> with the resulting amount and the same currency code.</returns>
    public FinancialAmount MultiplyBy(decimal factor) => new(Amount * factor, CurrencyCode);
}
