using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using iPath.Application.Coding;
using System.Text.Json;

namespace iPath.Application.Coding;

public class ValueSetTranslationService
{
    private readonly CodeSystemTranslationService _srv;
    private readonly ValueSet? _valueSet;
    private ValueSetDisplay? _display;

    public ValueSetTranslationService(CodeSystemTranslationService srv, CodeSystem codeSystem, ValueSet valueSet)
    {
        _srv = srv;
        _valueSet = valueSet;
        CodeSystem = codeSystem;
    }

    public ValueSet? ValueSet => _valueSet;


    private string? CodeSystemUrl => CodeSystem.Url;
    public readonly CodeSystem CodeSystem;


    public ValueSetDisplay? DisplaySet
    {
        get
        {
            if (_display == null && ValueSet is not null)
            {
                _display = new ValueSetDisplay(CodeSystem, ValueSet);
                LoadMissingDesignations(_display.DisplayTree);
            }
            return _display;
        }
    }


    private void LoadMissingDesignations(List<CodeDisplay>? items)
    {
        if (items is null) return;

        foreach (var item in items)
        {
            if (string.IsNullOrEmpty(item.Display))
            {
                item.Display = _srv.GetDesignation(CodeSystemUrl, ValueSet.Language, item.Code);
            }
            LoadMissingDesignations(item.Children);
        }
    }



    public ValueSet? TranslateValues(string lang)
    {
        if (ValueSet is not null && !string.IsNullOrEmpty(lang))
        {
            // Clone the ValueSet
            var options = new JsonSerializerOptions().ForFhir(ModelInfo.ModelInspector).Pretty();
            var json = JsonSerializer.Serialize(ValueSet, options);
            var copy = JsonSerializer.Deserialize<ValueSet>(json, options);


            // set language 
            copy.Language = lang;
            var dsp = copy.Expansion.Parameter.FirstOrDefault(x => x.Name == "displayLanguage");
            if (dsp is not null)
            {
                dsp.Value = new FhirString(lang);
            }

                        // find translations
            foreach (var item in copy.Expansion.Contains)
            {
                item.Display = _srv.GetDesignation(CodeSystemUrl, lang, item.Code);
            }

            return copy;
        }
        return null;
    }
}
