using iPath.Blazor.Componenents.Admin.Groups;
using iPath.Blazor.Componenents.Nodes;
using iPath.Domain.Entities;
using Microsoft.AspNetCore.Components;

namespace iPath.Blazor.Componenents.Admin.Users;

public partial class EditUserGroupMembershipDialog(GroupAdminViewModel gvm, UserAdminViewModel uvm)
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; }

    [Parameter]
    public Guid? selectedCommunityId { get; set; } = null;

    [Parameter]
    public bool ShowActiveOnly { get; set; } = true;


    IEnumerable<GroupListDto>? allGroups = null;
    List<GroupMemberModel>? members = null;
    List<GroupMemberModel>? displayedMembers = null;


    bool saving = false;

    protected override async Task OnParametersSetAsync()
    {
        await LoadData();

        if (ShowActiveOnly)
        {
            displayedMembers = members.Where(m => m.Role != eMemberRole.None).ToList();
        }
        else
        {
            displayedMembers = members;
        }
        StateHasChanged();
    }

    protected async Task LoadData()
    {
        // get list of all groups
        if (allGroups is null)
        {
            allGroups = await gvm.GetAllAsync();

            var tmp = new List<GroupMemberModel>();
            foreach (var item in allGroups.OrderBy(c => c.Name))
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
            members = tmp;
        }
    }

    async Task OnCommunitySelected()
    {
        await OnParametersSetAsync();
    }


    private void Cancel() => MudDialog.Cancel();

    async Task Save()
    {
        saving = true;
        StateHasChanged();

        try
        {
            var memberships = members.Select(m => m.ToDto()).ToArray();
            var cmd = new UpdateGroupMembershipCommand(uvm.SelectedItem.Id, memberships);
            await uvm.SaveGroupMemberships(cmd);

            MudDialog.Close();
        }
        catch (Exception ex)
        {
            return;
        }
        finally
        {
            saving = false;
        }
    }


    private class GroupMemberModel
    {
        public Guid GroupId { get; private set; }
        public string GroupName { get; private set; }
        public eMemberRole OriginalRole { get; private set; }
        public eMemberRole Role { get; set; }
        public bool HasChange => OriginalRole != Role;

        public bool IsModerator
        {
            get => Role == eMemberRole.Moderator;
            set => Role = value ? eMemberRole.Moderator : ToggleRole(Role);
        }
        public bool IsUser
        {
            get => Role == eMemberRole.User;
            set => Role = value ? eMemberRole.User : ToggleRole(Role);
        }
        public bool IsGuest
        {
            get => Role == eMemberRole.Guest;
            set => Role = value ? eMemberRole.Guest : ToggleRole(Role);
        }
        public bool IsBanned
        {
            get => Role == eMemberRole.Inactive;
            set => Role = value ? eMemberRole.Inactive : ToggleRole(Role);
        }
        private eMemberRole ToggleRole(eMemberRole input)
        {
            return Role == input ? eMemberRole.None : Role;
        }

        public GroupMemberModel(UserGroupMemberDto dto, string groupName)
        {
            GroupId = dto.GroupId;
            GroupName = groupName;
            OriginalRole = (eMemberRole)dto.Role;
            Role = (eMemberRole)dto.Role;
        }

        public GroupMemberModel(Guid groupId, string groupName)
        {
            GroupId = groupId;
            GroupName = groupName;
            OriginalRole = eMemberRole.None;
            Role = eMemberRole.None;
        }

        public UserGroupMemberDto ToDto()
        {
            return new UserGroupMemberDto(GroupId: this.GroupId, Role: this.Role, Groupname: null);
        }
    }
}