using Scalar.AspNetCore;

namespace iPath.API;

public static class TestEndpoints
{
    public static IEndpointRouteBuilder MapTestApi(this IEndpointRouteBuilder route)
    {
        route.MapPost("test/notify",
            async (TestEvent evt, IMediator mediator, CancellationToken ct)
                => mediator.Publish(evt, ct))
                .WithTags("Test")
                .RequireAuthorization();

        return route;
    }
}
