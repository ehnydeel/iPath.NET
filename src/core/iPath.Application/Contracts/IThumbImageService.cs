namespace iPath.Application.Contracts;

public interface IThumbImageService
{
    ValueTask Handle(DocumentThumnailNotCreatedNotification request, CancellationToken cancellationToken);
    Task<NodeFile> UpdateNodeAsync(NodeFile file, string filename);
}