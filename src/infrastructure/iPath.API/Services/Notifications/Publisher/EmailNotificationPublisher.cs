namespace iPath.API.Services.Notifications.Publisher;

public class EmailNotificationPublisher(IMediator mediator, IEmailSender email)
    : INotificationPublisher
{
    public eNotificationTarget Target => eNotificationTarget.Email;

    public async Task PublishAsync(Notification n, CancellationToken ct)
    {
        if (n.User is null || !n.User.EmailConfirmed)
        {
            n.MarkAsFailed("User has no confirmed email");
            return;
        }
        if (!n.ServiceRequestId.HasValue)
        {
            n.MarkAsFailed("Service Request not found");
            return;
        }

        var sr = await mediator.Send(new GetServiceRequestByIdQuery(n.ServiceRequestId.Value), ct);
        if (sr is null)
        {
            n.MarkAsFailed("Service Request not found");
            return;
        }

        var subject = n.EventType switch
        {
            Domain.Notificxations.eNodeNotificationType.NodePublished => "a new case has been published",
            Domain.Notificxations.eNodeNotificationType.NewAnnotation => "a new annotation has been published",
            _ => ""
        };


        var msg = "Dear " + n.User.UserName + ", " + subject;

        var body = template
                .Replace("{msg}", msg)
                .Replace("{title}", sr.Title);

        var desc = "";
        if (!string.IsNullOrEmpty(sr.Description.Text))
        {
            desc += sr.DescriptionHtml + "<br /><br />";
        }
        if (!string.IsNullOrEmpty(sr.Description.Questionnaire?.GeneratedText))
        {
            desc += sr.Description.Questionnaire?.GeneratedText + "<br /><br />";
        }
        body = body.Replace("{desc}", desc);

        // TODO: annotations

        var res = await email.SendMailAsync(n.User.Email, subject, desc);

        n.MarkAsSent();

        await Task.Delay(100);
    }




    const string template = """
        <!DOCTYPE html>

        <html lang="en" xmlns="http://www.w3.org/1999/xhtml">
        <head>
            <meta charset="utf-8" />
            <title>Notification</title>
            <style>
                .cont {
                    width: 100vh;
                    border: 1px solid;
                    margin: 1em;
                }
                .title {
                    background-color: grey;
                    font-weight: bold;
                    padding: 8px;
                    border-bottom: 1px inset;
                }
                .sender {
                    background-color: grey;
                    padding: 8px;
                    border-bottom: 1px solid;
                }
                .desc{
                    padding: 1em;
                }
                .comments {
                    border-top: 1px inset;
                    margin-top: 1em;
                    padding: 1em;
                }
            </style>
        </head>
            <body>
                <b>{msg}</b>

                <div class="cont">
                    <div class="title">{title}</div>
                    <div class="sender">{sender}</div>
                    <div class="desc">{description}</div>

                    <div class="comments">
                        <div>Comments</div>
                        {comments}
                    </div>
                </div>
            </body>
        </html>
""";
}



