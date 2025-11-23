using Scalar.AspNetCore;
using System.ComponentModel;
using System.Linq.Dynamic.Core;
using iPath.Application.Features.Users;
using iPath.RazorLib.Localization;
using iPath.Application.Localization;

namespace iPath.API;

public static class AdminEndpoints
{
    public static IEndpointRouteBuilder MapAdminApi(this IEndpointRouteBuilder route)
    {
        route.MapGet("admin/mailbox", 
            ([DefaultValue(0)] int page, [DefaultValue(10)] int pagesize, IEmailRepository repo, CancellationToken ct)
            => repo.GetPage(new PagedQuery<EmailMessage> { Page = page, PageSize = pagesize }, ct))
            .WithTags("Admin")
            // .RequireAuthorization()
            .Produces<PagedResult<EmailMessage>>();

        route.MapDelete("admin/mail/{id}", (string id, IEmailRepository repo, CancellationToken ct)
            => repo.Delete(Guid.Parse(id), ct))
            .WithTags("Admin");

        route.MapDelete("admin/mail/all", (IEmailRepository repo, CancellationToken ct)
            => repo.DeleteAll(ct))
            .WithTags("Admin");


        route.MapGet("session", (IUserSession sess) => sess.User)
            .Produces<SessionUserDto>()
            .WithTags("Session")
            .RequireAuthorization();


        route.MapGet("translations/{lang}", (string lang, LocalizationFileService srv)
            => srv.GetTranslationData(lang))
            .Produces<TranslationData>()
            .WithTags("Localization");


        return route;
    }
}
