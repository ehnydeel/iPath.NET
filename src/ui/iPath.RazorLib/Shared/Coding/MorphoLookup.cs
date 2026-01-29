namespace iPath.Blazor.Componenents.Shared.Coding;

public class MorphoLookup(IServiceProvider sp) : CodingLookup(sp)
{
    protected override void OnInitialized()
    {
        CodingService ??= "icdo";
        ValueSetId ??= "icdo-morpho";
        base.OnInitialized();
    }
}
