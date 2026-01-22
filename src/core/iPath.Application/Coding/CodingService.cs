using Ardalis.GuardClauses;
using Hl7.Fhir.Model;
using iPath.Application.Coding;
using iPath.Application.Fhir;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace iPath.Application.Coding;

public class CodingService
{
    private readonly ILogger<CodingService> logger;
    private readonly IFhirDataLoader loader;
    private readonly Dictionary<string, ValueSetDisplay> _valueSet = new();
    private readonly string _codeSystemId;
    private CodeSystem _codeSystem;

    public CodingService(IServiceProvider sp, string csKey)
    {
        logger = sp.GetRequiredService<ILogger<CodingService>>();
        loader = sp.GetRequiredService<IFhirDataLoader>();
        var config = sp.GetRequiredService<IConfiguration>();
        _codeSystemId = config[$"CodeSystems:{csKey}"] ?? csKey;
    }

    public CodingService(ILogger<CodingService> logger, IFhirDataLoader loader, string codeSystemId)
    {
        this.logger = logger;
        this.loader = loader;
        _codeSystemId = codeSystemId;
    }


    public async Task LoadCodeSystem()
    {
        if (_codeSystem is null && !string.IsNullOrEmpty(_codeSystemId))
        {
            _codeSystem = await loader.GetResourceAsync<CodeSystem>($"CodeSystem/{_codeSystemId}");   
        }
    }

    public CodeSystem CodeSystem => _codeSystem;
    public string CodeSystemUrl => _codeSystem is null ? "" : _codeSystem.Url.ToLowerInvariant().Trim();

    public async Task LoadValueSet(string id)
    {
        var vs = await loader.GetResourceAsync<ValueSet>($"ValueSet/{id}");
        if (vs is not null)
            LoadValueSet(vs, id);
    }


    public async Task LoadValueSet(ValueSet vs, string? vsId = null)
    {
        vsId ??= vs.Id;

        if (string.IsNullOrEmpty(vsId)) throw new ArgumentNullException("ValueSet.Id");
        vsId = vsId.ToLowerInvariant().Trim();

        // assert hat valueset beldongs to a code system
        var vsSystemUrl = vs.Compose?.Include.FirstOrDefault()?.System.ToLowerInvariant().Trim();
        if (string.IsNullOrEmpty(vsSystemUrl)) throw new ArgumentNullException("ValueSet.Compose.Include.System");

        // asser that coodesystem has been loaded
        if (CodeSystemUrl != vsSystemUrl) throw new ArgumentNullException("CodeSystem not loaded");

        var provider = new ValueSetDisplay(CodeSystem, vs);

        if (_valueSet.ContainsKey(vsId))
        {
            _valueSet[vsId] = provider;
        }
        else
        {
            _valueSet.Add(vsId, provider);
        }
    }


    public ValueSetDisplay? GetValueSetDisplay(string vsId)
    {
        if (!string.IsNullOrEmpty(vsId))
        {
            vsId = vsId.ToLowerInvariant().Trim();
            if (_valueSet.ContainsKey(vsId))
            {
                return _valueSet[vsId];
            }
        }
        return null;
    }
}