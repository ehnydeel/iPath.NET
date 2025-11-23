using DispatchR.Abstractions.Send;

namespace iPath.Application.Features.Nodes;

public record CreateNodeAnnotationCommand(Guid NodeId, string? Text, string? QuestionnaireResponse)
    : IRequest<CreateNodeAnnotationCommand, Task<AnnotationDto>>
    , IEventInput
{
    public string ObjectName => nameof(Node);
}


public static partial class NodeCommandExtensions
{
    public static Annotation CreateNodeAnnotation(this Node node, CreateNodeAnnotationCommand request, Guid userId)
    {
        var a = new Annotation
        {
            Text = request.Text,
            OwnerId = userId,
            CreatedOn = DateTime.UtcNow,
        };
        node.Annotations.Add(a);
        node.CreateEvent<AnnotationCreatedEvent, CreateNodeAnnotationCommand>(request, userId);
        return a;
    }
}

