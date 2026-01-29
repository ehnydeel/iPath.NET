
using DispatchR.Abstractions.Send;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace iPath.EF.Core.FeatureHandlers.Emails;

public class EmailRepository(iPathDbContext db, IEmailQueue queue)
    : IEmailRepository
{
    public async Task DeleteAll(CancellationToken ct)
    {
        await db.EmailStore.ExecuteDeleteAsync(ct);
    }

    public async Task Delete(Guid id, CancellationToken ct)
    {
        await db.EmailStore.Where(m => m.Id == id).ExecuteDeleteAsync(ct);
    }

    public async Task<PagedResultList<EmailMessage>> GetPage(PagedQuery<EmailMessage> query, CancellationToken ct)
    {
        var q = db.EmailStore.AsNoTracking()
            .ApplyQuery(query, "CreatedOn DESC");
        return await q.ToPagedResultAsync(query, ct);
    }

    public async Task<EmailMessage> Create([EmailAddress] string address, string subject, string body, CancellationToken ct)
    {
        var mail = new EmailMessage
        {
            Id = Guid.CreateVersion7(),
            CreatedOn = DateTime.UtcNow,
            Receiver = address,
            Subject = subject,
            Body = body
        };
        await db.EmailStore.AddAsync(mail, ct);
        await db.SaveChangesAsync(ct);
        await queue.EnqueueAsync(mail);
        return mail;
    }

    public async Task SetSent(Guid Id, CancellationToken ct)
    {
        var email = await db.EmailStore.FindAsync(Id, ct);
        if (email is not null)
        {
            email.SentOn = DateTime.UtcNow;
            await db.SaveChangesAsync(ct);
        }
    }

    public async Task SetError(Guid Id, string error, CancellationToken ct)
    {
        var email = await db.EmailStore.FindAsync(Id, ct);
        if (email is not null)
        {
            email.ErrorMessage = error;
            await db.SaveChangesAsync(ct);
        }
    }

    public async Task SetReadState(Guid Id, bool IsRead, CancellationToken ct)
    {
        var email = await db.EmailStore.FindAsync(Id, ct);
        if (email is not null)
        {
            email.IsRead = true;
            await db.SaveChangesAsync(ct);
        }
    }
}
