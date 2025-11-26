using iPath.Blazor.Componenents.Admin.Users;
using iPath.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace iPath.Blazor.Componenents.Admin.Groups;

public class GroupAdminViewModel(IPathApi api, 
    ISnackbar snackbar,
    IDialogService dialog,
    IMemoryCache cache,
    ILogger<GroupAdminViewModel> logger)
    : IViewModel
{
    public string SearchString { get; set; } = "";
    public async Task<GridData<GroupListDto>> GetListAsync(GridState<GroupListDto> state)
    {
        var query = state.BuildQuery(new GetGroupListQuery { AdminList = true, SearchString = this.SearchString });
        var resp = await api.GetGroupList(query);

        if (resp.IsSuccessful) return resp.Content.ToGridData();

        snackbar.AddWarning(resp.ErrorMessage);
        return new GridData<GroupListDto>();
    }



    const string groupListCacheKey = "admin.grouplist";
    private readonly SemaphoreSlim _cacheLock = new SemaphoreSlim(1);

    public async Task<IEnumerable<GroupListDto>> GetAllAsync()
    {
        try
        {
            await _cacheLock.WaitAsync();
            if (!cache.TryGetValue<IEnumerable<GroupListDto>>(groupListCacheKey, out var grouplist))
            {
                var query = new GetGroupListQuery { PageSize = null, AdminList = true };
                var resp = await api.GetGroupList(query);
                if (resp.IsSuccessful)
                {
                    grouplist = resp.Content.Items;

                    var opts = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(15));
                    cache.Set(groupListCacheKey, grouplist, opts);
                }
            }
            return grouplist;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
        }
        finally
        {
            _cacheLock.Release();
        }

        return null;
    }

    private void DeleteGroupListCache()
    {
        cache.Remove(groupListCacheKey);
    }



    public GroupListDto SelectedItem { get; private set; }
    public async Task SetSelectedItem(GroupListDto item)
    {
        if (SelectedItem == item) return;
        SelectedItem = item;
        await LoadGroup(item?.Id);
    }

    public string SelectedRowStyle(GroupListDto item, int rowIndex)
    {
        if (item is not null && SelectedItem is not null && item.Id == SelectedItem.Id )
            return "background-color: var(--mud-palette-background-gray)";

        return "";
    }



    public GroupDto SelectedGroup { get; private set; }
    private async Task LoadGroup(Guid? id)
    {
        if (!id.HasValue)
        {
            SelectedGroup = null;
        }
        else
        {
            var resp = await api.GetGroup(id.Value);
            if (resp.IsSuccessful)
            {
                SelectedGroup = resp.Content;
            }
            snackbar.AddError(resp.ErrorMessage);
        }
    }


    public async Task Create()
    {
        var p = new DialogParameters<CreateGroupDialog> { { x => x.Model, new GroupEditModel() } };
        DialogOptions opts = new() { MaxWidth = MaxWidth.Medium, FullWidth = false, NoHeader = false };
        var dlg = await dialog.ShowAsync<CreateGroupDialog>("Create a new group", options: opts, parameters: p);
        var res = await dlg.Result;
        var m = res?.Data as GroupEditModel;
        if (m != null)
        {
            var cmd = new CreateGroupCommand
            {
                Name = m.Name,
                OwnerId = m.Owner.Id,
                Settings = m.Settings,
                Visibility = eGroupVisibility.MembersOnly
            };
            var resp = await api.CreateGroup(cmd);
            if (!resp.IsSuccessful)
            {
                snackbar.AddWarning(resp.ErrorMessage);
            }
        }
    }

    public async Task Edit()
    {
        if (SelectedGroup != null)
        {
            var m = new GroupEditModel
            {
                Id = SelectedGroup.Id,
                Name = SelectedGroup.Name,
                Settings = SelectedGroup.Settings,
            };

            var p = new DialogParameters<EditGroupDialog> { { x => x.Model, m } };
            DialogOptions opts = new() { MaxWidth = MaxWidth.Medium, FullWidth = false, NoHeader = false };
            var dlg = await dialog.ShowAsync<EditGroupDialog>("Edit groups", options: opts, parameters: p);
            var res = await dlg.Result;
            var r = res?.Data as GroupEditModel;
            if (r != null && r.Id.HasValue)
            {
                var cmd = new UpdateGroupCommand
                {
                    Id = r.Id.Value,
                    Name = r.Name,
                    OwnerId = r.Owner.Id,
                    Settings = r.Settings
                };
                var resp = await api.UpdateGroup(cmd);
                if (!resp.IsSuccessful)
                {
                    snackbar.AddWarning(resp.ErrorMessage);
                }
            }
        }
    }

    public async Task Delete()
    {
        if (SelectedItem != null)
        {
            snackbar.AddWarning("not implemented yet");
        }
    }


    public async Task AddToCommunity(CommunityListDto group)
    {
        snackbar.AddWarning("not implemented yet");
    }

    public async Task RemnoveFromCommunity(CommunityListDto group)
    {
        snackbar.AddWarning("not implemented yet");
    }
}


public class GroupEditModel
{
    public Guid? Id { get; set; }
    [Required]
    public string Name { get; set; } = "";
    public GroupSettings Settings { get; set; } = new();
    public UserListDto? Owner { get; set; }
}
