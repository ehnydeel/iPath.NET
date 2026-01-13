using iPath.Application.Features;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;


namespace iPath.Blazor.Server.Components.Account
{
    internal sealed class IdentityQueuedSender(IEmailRepository repo, NavigationManager nm) : IEmailSender<User>
    {

        private async Task Enqueue(string address, string subject, string body)
            => await repo.Create(address, subject, body, default);

        public Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
            => Enqueue(email, "Confirm your email",
                $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");


        public Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
        {
            string baseAddress = nm.ToAbsoluteUri("").AbsoluteUri;

            if (resetLink.Contains("migrate="))
            {
                // migration confirmation
                return Enqueue(email, "iPath Account Migration",
                    $"Please confirm your account migration by <a href='{resetLink}'>clicking here</a>.");
            }
            else
            {
                return Enqueue(email, "Reset your iPath password",
                    $"""
                    <p>A request to reset your password has been received on <a href='{baseAddress}'>iPath-Server</a>.</p>
                    <p>If you have asked for resetting your password, please follow this link and set a new password</p>
                    <ul>
                        <li><a href='{resetLink}'>reset password</a></li>
                    </ul>
                    """);
            }
        }

        public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
            => Enqueue(email, "Reset your password",
                $"Please reset your password using the following code: {resetCode}");
    }
}
