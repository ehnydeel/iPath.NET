using iPath.Application.Contracts;
using Microsoft.Extensions.Logging;

namespace iPath.Blazor.Componenents.Shared;

public class AppState(IPathApi api, ILogger<AppState> logger) : IUserSession
{
    private SessionUserDto _user;

    public SessionUserDto? User => _user;
    public bool IsAuthenticated => _user is not null && _user.Id != Guid.Empty;


    public async Task ReloadSession()
    {
        _user = SessionUserDto.Anonymous;
        try
        {
            var resp = await api.GetSession();
            if (resp.IsSuccessful)
            {
                _user = resp.Content;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "error calling /api/v1/session");
        }
    }

    public void ReloadUser(Guid userId)
    {
        _user = SessionUserDto.Anonymous;
    }

    public Color PresenceColor => Color.Success;
}
