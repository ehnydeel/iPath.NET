namespace iPath.Application.Features.Nodes;


public record UploadNodeFileCommand(Guid RootNodeId, Guid ParentNodeId,
    string filename, long fileSize, Stream fileStream)
    : IRequest<UploadNodeFileCommand, Task<NodeDto>>;



public record UploadNodeFileInput(Guid RootNodeId, Guid ParentNodeId, string filename) : IEventInput
{
    public string ObjectName => nameof(Node);
}