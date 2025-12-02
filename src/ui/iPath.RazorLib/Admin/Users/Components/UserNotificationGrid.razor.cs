using Humanizer;

namespace iPath.Blazor.Componenents.Admin.Users;

public partial class UserNotificationGrid(UserAdminViewModel vm, IPathApi api, ISnackbar snackbar, IDialogService dialog)
{
    [Parameter]
    public UserDto? User { get; set; }


    List<UserNotificationModel> Items = new();


    protected override async Task OnParametersSetAsync()
    {        
        if (User is not null)
        {
            var resp = await api.GetUserNotification(vm.SelectedUser.Id);
            if( !resp.IsSuccessful )
            {
                snackbar.AddError(resp.ErrorMessage);
                return;
            }

            Items = resp.Content.Select(x => new UserNotificationModel(x)).ToList();
        }
    }

    async Task Save()
    {
        try
        {
            var dtos = Items.Select(n => n.ToDto()).ToArray();
            var cmd = new UpdateUserNotificationsCommand(vm.SelectedUser.Id, dtos);
            var resp = await api.UpdateUserNotification(cmd);
            if (!resp.IsSuccessful) 
            {
                snackbar.AddError(resp.ErrorMessage);
            }
        }
        catch(Exception ex)
        {
            snackbar.AddError(ex.Message);
        }
    }

    async Task ShowSettings(UserNotificationModel model)
    {
        if (model is not null)
        {
            var p = new DialogParameters<NotificationSettingsDialog> { { x => x.Model, model.Settings } };
            var o = new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true };
            var dlg = dialog.ShowAsync<NotificationSettingsDialog>("Settings", options: o, parameters: p);
            model.UpdateHasSettings();
            StateHasChanged();
        }
    }
}


public class UserNotificationModel
{
    public readonly UserGroupNotificationDto Dto;

    public UserNotificationModel(UserGroupNotificationDto dto)
    {
        Dto = dto;

        NewCase =  dto.Source.HasFlag(eNotificationSource.NewCase);
        NewAnnotation = dto.Source.HasFlag(eNotificationSource.NewAnnotation);
        NewAnnotationOnMyCase = dto.Source.HasFlag(eNotificationSource.NewAnnotationOnMyCase);

        InApp = dto.Tartget.HasFlag(eNotificationTarget.InApp);
        Email = dto.Tartget.HasFlag(eNotificationTarget.Email);

        Settings = dto.Settings ?? new();
        UpdateHasSettings();
    }

    public Guid GroupId => Dto.GroupId;
    public string Groupname => Dto.Groupname;

    public NotificationSettings Settings { get; private set; }

    public bool NewCase { get; set; }
    public bool NewAnnotation { get; set; }
    public bool NewAnnotationOnMyCase { get; set; }

    public bool InApp {  get; set; }
    public bool Email { get; set; }


    public bool HasSettings { get; set; }
    public void UpdateHasSettings()
    {
        if (Dto.Settings != null)
        {
            HasSettings = !String.IsNullOrEmpty(Dto.Settings?.IcdoTopoCode) || Dto.Settings.DailyEmailSummary;
        }
    }


    public string SettingsIcon => HasSettings ? Icons.Material.Filled.SettingsSuggest : Icons.Material.Filled.Settings;


    public UserGroupNotificationDto ToDto()
    {
        eNotificationSource source = eNotificationSource.None;
        if (NewCase) source |= eNotificationSource.NewCase;
        if (NewAnnotation) source |= eNotificationSource.NewAnnotation;
        if (NewAnnotationOnMyCase) source |= eNotificationSource.NewAnnotationOnMyCase;

        eNotificationTarget target = eNotificationTarget.None;
        if (InApp) target |= eNotificationTarget.InApp;
        if (Email) target |= eNotificationTarget.Email;

        var res = Dto with { Source = source, Tartget = target, Settings = this.Settings };
        return res;
    }
}