using iPath.Application.Features;
using Microsoft.AspNetCore.Identity;


namespace iPath.Blazor.Server.Components.Account
{
    internal sealed class IdentityQueuedSender(IEmailRepository repo) : IEmailSender<User>
    {

        private async Task Enqueue(string address, string subject, string body)
            => await repo.Create(address, subject, body, default);

        public Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
            => Enqueue(email, "Confirm your email",
                $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");


        public Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
        {
            if (resetLink.Contains("email="))
            {
                // migration confirmation
                return Enqueue(email, "iPath Account Migration",
                    $"Please confirm your account migration by <a href='{resetLink}'>clicking here</a>.");
            }
            else
            {
                return Enqueue(email, "Reset your iPath password",
                    $"Please reset your password by <a href='{resetLink}'>clicking here</a>.");
            }
        }

        public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
            => Enqueue(email, "Reset your password",
                $"Please reset your password using the following code: {resetCode}");
    }
}
