namespace iPath.Blazor.Componenents.Shared.Lookups;

public class CountryLookup : MudAutocomplete<string>
{
    protected override void OnInitialized()
    {
        this.SearchFunc = (string? term, CancellationToken ct) => SearchCountry(term, ct);
    }

    private async Task<IEnumerable<string>> SearchCountry(string value, CancellationToken ctk)
    {
        if (string.IsNullOrEmpty(value)) return iPath.Domain.CountryService.CountryNames;
        return iPath.Domain.CountryService.CountryNames.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
}
