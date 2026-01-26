using iPath.API.Endpoints;
using iPath.API.Middleware;
using Microsoft.Extensions.Configuration;
using Scalar.AspNetCore;

namespace iPath.API;

public static class MapEndpoints
{
    public static IEndpointRouteBuilder MapIPathApi(this WebApplication app, IConfiguration config)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();
        // app.UseResponseCompression();

        var route = app.MapGroup("api/v1");
        route.MapAdminApi()
            .MapUsersApi()
            .MapCommunitiesApi()
            .MapGroupsApi()
            .MapServiceRequestEndpoints()
            .MapDocumentEndpoints()
            .MapNotificationApi()
            .MapQuesionnairesApi()
            .MapFhirApi()
            .MapTestApi()
            .MapIPathHubs();

        // OpenAPI Documentation
        var baseAddress = config["BaseAddress"];
        app.MapOpenApi("/openapi/v1.json");
        app.MapScalarApiReference(opts =>
        {
            if (!string.IsNullOrEmpty(baseAddress))  opts.BaseServerUrl = baseAddress;
        });

        return route;
    }
}
