using Microsoft.AspNetCore.Components;

namespace iPath.Blazor.Componenents.Documents;

public partial class UploadGalleryItem : IDisposable
{
    [Parameter, EditorRequired]
    public UploadTask Upload { get; set; }

    protected override void OnParametersSet()
    {
        Upload.OnChange += StateHasChanged;
    }

    public void Dispose()
    {
        if (Upload is not null)
            Upload.OnChange -= StateHasChanged; ;
    }
}