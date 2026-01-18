
using Ardalis.GuardClauses;
using iPath.Application.Features.Documents;
using Microsoft.AspNetCore.Mvc;

namespace iPath.API.Endpoints;

public static class NodeEndpoints
{
    public static IEndpointRouteBuilder MapNodeEndpoints(this IEndpointRouteBuilder builder)
    {
        var grp = builder.MapGroup("nodes")
            .WithTags("Nodes");

        // Queries

        grp.MapGet("{id}", async (string id, [FromServices] IMediator mediator, CancellationToken ct)
            => await mediator.Send(new GetServiceRequestByIdQuery(Guid.Parse(id)), ct))
            .Produces<ServiceRequestDto>()
            .RequireAuthorization();

        grp.MapPost("list", async (GetServiceRequestsQuery request, [FromServices] IMediator mediator, CancellationToken ct)
            => await mediator.Send(request, ct))
            .Produces<PagedResultList<ServiceRequestListDto>>()
            .RequireAuthorization();

        grp.MapPost("idlist", async (GetServiceRequestIdListQuery request, [FromServices] IMediator mediator, CancellationToken ct)
            => await mediator.Send(request, ct))
            .Produces<IReadOnlyList<Guid>>()
            .RequireAuthorization();

        grp.MapGet("file/{id}/{filename}", async (string id, string? filename, [FromServices] IMediator mediator, HttpContext ctx, CancellationToken ct) =>
        {
            if (Guid.TryParse(id, out var nodeId))
            {
                var res = await mediator.Send(new GetDocumentFileQuery(nodeId), ct);

                if (res.NotFound)
                {
                    return Results.NotFound();
                }
                else if (res.AccessDenied)
                {
                    return Results.Unauthorized();
                }
                else
                {
                    return Results.File(res.TempFile, contentType: res.Info.MimeType);
                }
            }

            return Results.BadRequest();
        });
           // .RequireAuthorization();




        // Commands
        grp.MapPost("create", async (CreateServiceRequestCommand request, [FromServices] IMediator mediator, CancellationToken ct)
            => await mediator.Send(request, ct))
            .Produces<ServiceRequestDto>()
            .RequireAuthorization();

        grp.MapDelete("{id}", async (string id, [FromServices] IMediator mediator, CancellationToken ct)
            => await mediator.Send(new DeleteServiceRequestCommand(Guid.Parse(id)), ct))
            .Produces<ServiceRequestDeletedEvent>()
            .RequireAuthorization();

        grp.MapPut("update", async (UpdateServiceRequestCommand request, [FromServices] IMediator mediator, CancellationToken ct)
            => await mediator.Send(request, ct))
            .Produces<bool>()
            .RequireAuthorization();

        grp.MapPut("order", async (UpdateDcoumentsSortOrderCommand request, [FromServices] IMediator mediator, CancellationToken ct)
            => await mediator.Send(request, ct))
            .Produces<ChildNodeSortOrderUpdatedEvent>()
            .RequireAuthorization();

        grp.MapPost("visit/{id}", async (string id, [FromServices] IMediator mediator, CancellationToken ct)
            => await mediator.Send(new UpdateServiceRequestVisitCommand(Guid.Parse(id)), ct))
            .Produces<bool>()
            .RequireAuthorization();

        grp.MapPost("{requestId}/upload/{parentId}", async (string requestId, string? parentId, [FromForm] IFormFile file, 
            [FromServices] IMediator mediator, CancellationToken ct) =>
        {
            if (file is not null)
            {
                var fileName = file.FileName;
                var fileSize = file.Length;
                var contentType = file.ContentType;

                Guard.Against.Null(fileSize);

                if (Guid.TryParse(requestId, out var requestGuid))
                {
                    await using Stream stream = file.OpenReadStream();
                    var req = new UploadDocumentCommand(RequestId: requestGuid, ParentId: Guid.Parse(parentId) , filename: fileName, fileSize: fileSize, fileStream: stream, contenttype: contentType);
                    var node = await mediator.Send(req, ct);
                    return node is null ? Results.NoContent() : Results.Ok(node);
                }
                else
                {
                    return Results.NotFound();
                }
            }
            return Results.NoContent();
        })
            .DisableAntiforgery()
            .Produces<DocumentDto>()
            .RequireAuthorization();


        grp.MapPost("annotation", async (CreateAnnotationCommand request, [FromServices] IMediator mediator, CancellationToken ct)
            => await mediator.Send(request, ct))
            .Produces<AnnotationDto>()
            .RequireAuthorization();

        grp.MapDelete("annotation/{id}", async (string id, [FromServices] IMediator mediator, CancellationToken ct)
            => await mediator.Send(new DeleteAnnotationCommand(Guid.Parse(id)), ct))
            .Produces<Guid>()
            .RequireAuthorization();


        return builder;
    }
}
