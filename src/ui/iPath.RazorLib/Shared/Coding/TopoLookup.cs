namespace iPath.Blazor.Componenents.Shared.Coding;

public class TopoLookup(IServiceProvider sp) : CodingLookup(sp)
{
    protected override void OnInitialized()
    {
        CodingService = "icdo";
        ValueSetId = "icdo-topo";
        base.OnInitialized();
    }
}
