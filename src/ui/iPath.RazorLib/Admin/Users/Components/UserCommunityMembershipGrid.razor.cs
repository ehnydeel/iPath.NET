using iPath.Blazor.Componenents.Admin.Communities;

namespace iPath.Blazor.Componenents.Admin.Users;

public partial class UserCommunityMembershipGrid(CommunityAdminViewModel cvm, UserAdminViewModel uvm)
{
    [Parameter]
    public UserDto? User { get; set; }

    [Parameter]
    public Guid? selectedCommunityId { get; set; } = null;

    [Parameter]
    public bool ShowActiveOnly { get; set; } = false;

    List<CommunityMemberModel>? allMemberShips = null;
    List<CommunityMemberModel>? activeMemberShips = null;


    Color SaveButtonColor => allMemberShips.Any(m => m.HasChange) ? Color.Primary : Color.Default;



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
        if (uvm.SelectedUser is not null)
        {
            var tmp = new List<CommunityMemberModel>();
            foreach (var item in (await cvm.GetAllAsync()).OrderBy(c => c.Name))
            {
                var m = uvm.SelectedUser.CommunityMembership.FirstOrDefault(m => m.CommunityId == item.Id);
                if (m != null)
                {
                    tmp.Add(new CommunityMemberModel(m, item.Name, uvm.SelectedUser.Id, uvm.SelectedUser.Username));
                }
                else
                {
                    tmp.Add(new CommunityMemberModel(item.Id, item.Name, uvm.SelectedUser.Id, uvm.SelectedUser.Username));
                }
            }
            allMemberShips = tmp;
        }
    }

    async Task Save()
    {
        if (uvm.SelectedUser is not null)
        {
            var memberships = allMemberShips.Select(m => m.ToDto()).ToArray();
            var cmd = new UpdateCommunityMembershipCommand(uvm.SelectedUser.Id, memberships);
            await uvm.UpdateCommunityMemberships(cmd);
            await LoadData();
        }
    }
}