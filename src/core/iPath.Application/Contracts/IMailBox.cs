using iPath.Application.Features;

namespace iPath.Application.Contracts;

public interface IMailBox
{
    IEnumerable<EmailDto> GetAllMails();
    IEnumerable<EmailDto> GetUnreadMails();
}