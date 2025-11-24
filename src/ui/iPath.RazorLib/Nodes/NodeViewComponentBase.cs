using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace iPath.Blazor.Componenents.Nodes;

public abstract class NodeViewComponentBase : ComponentBase, IDisposable
{
    [Inject] 
    protected NodeViewModel vm { get; set; }

    [Inject]
    protected IStringLocalizer T { get; set; }

    [Inject]
    protected ISnackbar snackbar { get; set; }

    [Inject]
    protected IDialogService dialogService { get; set; }


    protected override void OnInitialized()
    {
        vm.OnChange += OnChangedHandler;
    }

    void IDisposable.Dispose()
    {
        vm.OnChange -= OnChangedHandler;
        OnDisposed();
    }

    protected virtual void OnDisposed() { }

    protected virtual void OnChangedHandler()
    {
        InvokeAsync(() => StateHasChanged());
    }
}
