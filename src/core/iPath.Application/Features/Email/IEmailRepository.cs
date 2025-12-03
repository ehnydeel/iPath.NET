using System.ComponentModel.DataAnnotations;

namespace iPath.Application.Features;


public interface IEmailRepository
{
    Task<PagedResultList<EmailMessage>> GetPage(PagedQuery<EmailMessage> query, CancellationToken ct);
    Task Delete(Guid id, CancellationToken ct);
    Task DeleteAll(CancellationToken ct);

    Task<EmailMessage> Create([EmailAddress] string address, string subject, string body, CancellationToken ct);
    Task SetSent(Guid Id, CancellationToken ct);
    Task SetError(Guid Id, string error, CancellationToken ct);
    Task SetReadState(Guid Id, bool IsRead, CancellationToken ct);
}

