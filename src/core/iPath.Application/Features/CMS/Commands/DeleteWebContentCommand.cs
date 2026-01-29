namespace iPath.Application.Features.CMS;

public record DeleteWebContentCommand(Guid Id)
    : IRequest<DeleteWebContentCommand, Task>;