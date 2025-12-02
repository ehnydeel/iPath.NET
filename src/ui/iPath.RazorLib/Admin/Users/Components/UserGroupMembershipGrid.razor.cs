using iPath.Blazor.Componenents.Admin.Groups;

namespace iPath.Blazor.Componenents.Admin.Users;

public partial class UserGroupMembershipGrid(GroupAdminViewModel gvm, UserAdminViewModel uvm)
{
    [Parameter]
    public UserDto? User { get; set; }

    [Parameter]
    public Guid? selectedCommunityId { get; set; } = null;

    [Parameter]
    public bool ShowActiveOnly { get; set; } = true;

    List<GroupMemberModel>? allMemberShips = null;
    List<GroupMemberModel>? activeMemberShips = null;



    protected override async Task OnParametersSetAsync()
    {
        if (User is not null)
        {
            await LoadData();
            OnActiveOnlyChanged();
        }
    }

    void OnActiveOnlyChanged()
    {
        if (ShowActiveOnly)
        {
            activeMemberShips = allMemberShips.Where(m => m.Role != eMemberRole.None).ToList();
        }
        else
        {
            activeMemberShips = allMemberShips;
        }
        StateHasChanged();
    }



    protected async Task LoadData()
    {
        var tmp = new List<GroupMemberModel>();
        foreach (var item in (await gvm.GetAllAsync()).OrderBy(c => c.Name))
        {
            var m = uvm.SelectedUser.GroupMembership.FirstOrDefault(m => m.GroupId == item.Id);
            if (m != null)
            {
                tmp.Add(new GroupMemberModel(m, item.Name));
            }
            else
            {
                tmp.Add(new GroupMemberModel(item.Id, item.Name));
            }
        }
        allMemberShips = tmp;
    }

    async Task Save()
    {
        try
        {
            var memberships = allMemberShips.Select(m => m.ToDto()).ToArray();
            var cmd = new UpdateGroupMembershipCommand(uvm.SelectedUser.Id, memberships);
            await uvm.SaveGroupMemberships(cmd);
        }
        catch (Exception ex)
        {
            return;
        }
    }
}