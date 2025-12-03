using System.ComponentModel.DataAnnotations;

namespace iPath.API.Services.Email;

public class QueueEmailSender(IEmailRepository repo) : IEmailSender
{
    public async Task<Result> SendMailAsync([EmailAddress] string address, string subject, string body, CancellationToken ct = default)
    {
        var msg = await repo.Create(address, subject, body, ct);
        return Result.Ok();
    }
}
