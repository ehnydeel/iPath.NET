using Microsoft.Extensions.Localization;

namespace iPath.Blazor.Componenents.Admin.Groups;

public partial class GroupAdminPage(GroupAdminViewModel vm, IStringLocalizer T) : IDisposable
{
    int[] pageOpts => new[] { 25, 50, 100 };

    GroupAdminDetailView details;


    protected override void OnInitialized()
    {
        vm.OnChange += UpdateView;
    }

    public void Dispose()
    {
        vm.OnChange -= UpdateView;
    }


    void UpdateView()
    {
        InvokeAsync(() =>
        {
            if (details != null)
            {
                details.Model = vm.SelectedGroup;
            }
            StateHasChanged();
        });
    }
}