namespace iPath.Application.Features.ServiceRequests;


public record UpdateServiceRequestVisitCommand(Guid NodeId) : IRequest<UpdateServiceRequestVisitCommand, Task<bool>>;

