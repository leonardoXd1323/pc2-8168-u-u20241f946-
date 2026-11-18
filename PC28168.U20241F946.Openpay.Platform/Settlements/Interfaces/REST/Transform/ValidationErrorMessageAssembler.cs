using PC28168.U20241F946.Openpay.Platform.Resources;
using Microsoft.Extensions.Localization;

namespace PC28168.U20241F946.Openpay.Platform.Settlements.Interfaces.REST.Transform;

/// <summary>
///     Assembles localized error messages from the argument exceptions raised by the domain rules.
/// </summary>
/// <remarks>
///     The localization key is built as <c>{ExceptionType}.{ParamName}</c>, for example
///     <c>ArgumentOutOfRangeException.User</c>. Author: Sebastian Lopez
/// </remarks>
public static class ValidationErrorMessageAssembler
{
    /// <summary>
    ///     Assembles the localized message of an argument exception.
    /// </summary>
    /// <param name="exception">The argument exception raised by a domain rule.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <returns>The localized message, or a generic localized message when the exception is not mapped.</returns>
    public static string ToMessageFromException(ArgumentException exception,
        IStringLocalizer<SharedResource> localizer)
    {
        var localized = localizer[$"{exception.GetType().Name}.{exception.ParamName}"];
        return localized.ResourceNotFound ? localizer["InvalidDisbursementOrderRequest"].Value : localized.Value;
    }
}
