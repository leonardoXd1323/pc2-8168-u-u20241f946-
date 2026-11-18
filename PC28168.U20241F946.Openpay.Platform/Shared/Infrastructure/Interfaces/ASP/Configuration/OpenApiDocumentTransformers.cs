using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace PC28168.U20241F946.Openpay.Platform.Shared.Infrastructure.Interfaces.ASP.Configuration;

/// <summary>
///     Document-level transformers that enrich the generated OpenAPI document.
/// </summary>
/// <remarks>
///     All the texts of the OpenAPI documentation are written in English. Author: Sebastian Lopez
/// </remarks>
public static class OpenApiDocumentTransformers
{
    /// <summary>
    ///     Sets the general information (title, version, description and contact) of the API document.
    /// </summary>
    /// <param name="document">The OpenAPI document being generated.</param>
    /// <param name="context">The document transformer context.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A completed task.</returns>
    public static Task AddApiInformationAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        document.Info = new OpenApiInfo
        {
            Title = "Openpay Platform - Settlements API",
            Version = "v1",
            Description =
                "RESTful API that supports the settlement and fund transfer operations of Openpay Peru. " +
                "It lets operators register disbursement orders for affiliate stores. " +
                "Error messages are localized with the Accept-Language header (en, en-US, es, es-PE).",
            Contact = new OpenApiContact
            {
                Name = "Sebastian Lopez (U20241F946)"
            }
        };
        return Task.CompletedTask;
    }
}
