using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

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
        if (SelectedItem != null)
        {
            snackbar.AddWarning("not implemented yet");
        }
    }

    public async Task Edit()
    {
        if (SelectedItem != null)
        {
            snackbar.AddWarning("not implemented yet");
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
