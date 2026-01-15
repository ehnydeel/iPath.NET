using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace iPath.Application.Coding;

public class CodingService(ILogger<CodingService> logger)
{
    private readonly Dictionary<string, CodeSystem> _codeSystems = new();
    private readonly Dictionary<string, CodeSystem> _designations = new();
    private readonly Dictionary<string, ValueSetProvider> _valueSet = new();

    public CodeSystem LoadCodeSystem(string codeSystemJson)
    {
        var options = new JsonSerializerOptions().ForFhir(ModelInfo.ModelInspector);
        var cs = JsonSerializer.Deserialize<CodeSystem>(codeSystemJson, options);
        var url = cs?.Url.ToLowerInvariant().Trim();
        if (string.IsNullOrEmpty(url)) throw new ArgumentNullException("CodeSystem.Url");

        if (!_codeSystems.ContainsKey(url))
        {
            _codeSystems.Add(url, cs);
        }
        else
        {
            _codeSystems[url] = cs;
        }
        return _codeSystems[url];
    }

    public CodeSystem? DefaultCodeSystem => _codeSystems.Values.FirstOrDefault();
    public CodeSystem? GetCodeSysten(string url) => _codeSystems[url.ToLowerInvariant().Trim()];



    public CodeSystem LoadDesignationsSystem(string codeSystemJson, string codeSystemUrl)
    {
        var options = new JsonSerializerOptions().ForFhir(ModelInfo.ModelInspector);
        var cs = JsonSerializer.Deserialize<CodeSystem>(codeSystemJson, options);
        var url = codeSystemUrl.ToLowerInvariant().Trim();
        if (string.IsNullOrEmpty(url)) throw new ArgumentNullException("codeSystemUrl");

        if (!_designations.ContainsKey(url))
        {
            _designations.Add(url, cs);
        }
        else
        {
            _designations[url] = cs;
        }
        return _designations[url];
    }

    public string GetDesignation(string codeSystemUrl, string language, string code)
    {
        codeSystemUrl = codeSystemUrl.ToLowerInvariant().Trim();
        if (_designations.ContainsKey(codeSystemUrl))
        {
            var ds = _designations[codeSystemUrl];
            if (ds is not null)
            {
                var concept = ds.Concept.FirstOrDefault(c => c.Code == code);
                if (concept is not null)
                {
                    var des = concept.Designation.FirstOrDefault(d => d.Language.ToLower() == language.ToLower());
                    if (des is not null && !string.IsNullOrEmpty(des.Value))
                    {
                        return des.Value;
                    }
                }
                // fallback to display
                return concept.Display;
            }
        }
        return string.Empty;
    }


    public void LoadValueSet(string valueSetJson, string? vsId = null)
    {
        var options = new JsonSerializerOptions().ForFhir(ModelInfo.ModelInspector);
        var vs = JsonSerializer.Deserialize<ValueSet>(valueSetJson, options);
        LoadValueSet(vs, vsId);
    }


    public void LoadValueSet(ValueSet vs, string? vsId = null)
    {
        vsId ??= vs.Id;

        if (string.IsNullOrEmpty(vsId)) throw new ArgumentNullException("ValueSet.Id");
        vsId = vsId.ToLowerInvariant().Trim();

        // assert hat valueset beldongs to a code system
        var vsSystemUrl = vs.Compose?.Include.FirstOrDefault()?.System.ToLowerInvariant().Trim();
        if (string.IsNullOrEmpty(vsSystemUrl)) throw new ArgumentNullException("ValueSet.Compose.Include.System");

        // asser that coodesystem has been loaded
        if (!_codeSystems.ContainsKey(vsSystemUrl)) throw new ArgumentNullException("CodeSystem not loaded");

        var provider = new ValueSetProvider(this, _codeSystems[vsSystemUrl], vs);

        if (_valueSet.ContainsKey(vsId))
        {
            _valueSet[vsId] = provider;
        }
        else
        {
            _valueSet.Add(vsId, provider);
        }
    }


    public ValueSetProvider? GetValueSet(string vsId)
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