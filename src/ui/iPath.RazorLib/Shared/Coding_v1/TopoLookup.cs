namespace iPath.Blazor.Componenents.Shared;

public class TopoLookup(CodingService_v1 srv)
    : MudAutocomplete<CodedConcept>
{
    protected override void OnInitialized()
    {  
        this.ToStringFunc = x => x is null ? "" : $"{x.Display} [{x.Code}]";
        this.SearchFunc = Search; // (string? term, CancellationToken ct) => Search(term, ct);
        base.OnInitialized();
    }

    async Task<IEnumerable<CodedConcept>> Search(string? term, CancellationToken ct)
    {
        return await srv.FindTopoConcetps(term, ct);
    }
}
