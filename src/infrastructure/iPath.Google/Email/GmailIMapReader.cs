using iPath.Application.Contracts;
using iPath.Application.Features;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;

namespace iPath.Google.Email;


public class GmailIMapReader(IOptions<GmailConfig> opts) : IMailBox
{
    private ImapClient CreateClient()
    {
        var client = new ImapClient();
        client.Connect(opts.Value.ImapServer, opts.Value.ImapPort, true);

        // Note: since we don't have an OAuth2 token, disable
        // the XOAUTH2 authentication mechanism.
        client.AuthenticationMechanisms.Remove("XOAUTH2");

        client.Authenticate(opts.Value.AppUsername, opts.Value.AppPassword);
        return client;
    }


    public IEnumerable<EmailDto> GetUnreadMails()
    {
        var messages = new List<EmailDto>();

        using (var client = CreateClient())
        {
            // The Inbox folder is always available on all IMAP servers...
            var inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadOnly);
            var results = inbox.Search(SearchOptions.All, SearchQuery.Not(SearchQuery.Seen));
            foreach (var uniqueId in results.UniqueIds)
            {
                var message = inbox.GetMessage(uniqueId);

                var dto = new EmailDto
                {
                    Subject = message.Subject,
                    Address = message.From.FirstOrDefault()?.Name,
                    Body = message.HtmlBody
                };

                messages.Add(dto);

                //Mark message as read
                //inbox.AddFlags(uniqueId, MessageFlags.Seen, true);
            }

            client.Disconnect(true);
        }

        return messages;
    }

    public IEnumerable<EmailDto> GetAllMails()
    {
        var messages = new List<EmailDto>();

        using (var client = CreateClient())
        {
            // The Inbox folder is always available on all IMAP servers...
            var inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadOnly);
            var results = inbox.Search(SearchOptions.All, SearchQuery.NotSeen);
            foreach (var uniqueId in results.UniqueIds)
            {
                var message = inbox.GetMessage(uniqueId);

                var dto = new EmailDto
                {
                    Subject = message.Subject,
                    Address = message.From.FirstOrDefault()?.Name,
                    Body = message.HtmlBody
                };

                messages.Add(dto);

                //Mark message as read
                //inbox.AddFlags(uniqueId, MessageFlags.Seen, true);
            }

            client.Disconnect(true);
        }

        return messages;
    }
}
