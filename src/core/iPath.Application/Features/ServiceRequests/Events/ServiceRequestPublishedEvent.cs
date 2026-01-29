using iPath.Domain.Notificxations;

namespace iPath.Application.Features.ServiceRequests;

public class ServiceRequestPublishedEvent : ServiceRequestEvent, IEventWithNotifications;