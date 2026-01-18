using DispatchR.Abstractions.Send;

namespace iPath.Application.Features.ServiceRequests;

public record CreateAnnotationCommand(Guid requestId, AnnotationData? Data, Guid? docuemntId = null)
    : IRequest<CreateAnnotationCommand, Task<AnnotationDto>>
    , IEventInput
{
    public string ObjectName => nameof(ServiceRequest);
}


public static partial class ServiceRequestCommandExtensions
{
    public static Annotation CreateNodeAnnotation(this ServiceRequest node, CreateAnnotationCommand request, Guid userId)
    {
        var a = new Annotation
        {
            Data = request.Data,
            OwnerId = userId,
            ServiceRequestId = request.requestId,
            DcoumentNodeId = request.docuemntId,
            CreatedOn = DateTime.UtcNow,
        };
        node.Annotations.Add(a);
        node.CreateEvent<AnnotationCreatedEvent, CreateAnnotationCommand>(request, userId);
        return a;
    }
}

