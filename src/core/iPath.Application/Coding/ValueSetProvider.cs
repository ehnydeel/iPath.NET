using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using System.Diagnostics;
using System.Text.Json;

namespace iPath.Application.Coding;

public class ValueSetProvider
{
    private readonly CodingService _srv;
    private readonly ValueSet? _valueSet;

    public ValueSetProvider(CodingService srv, CodeSystem codeSystem, ValueSet valueSet)
    {
        _srv = srv;
        _valueSet = valueSet;
        CodeSystem = codeSystem;
    }

    public ValueSet? ValueSet => _valueSet;


    private string? CodeSystemUrl => CodeSystem.Url;
    public readonly CodeSystem CodeSystem;


    private List<CodeDisplay> _codes;

    public List<CodeDisplay> Displays
    {
        get
        {
            if (_codes is null)
            {
                _codes = new List<CodeDisplay>();

                var _values = ValueSet.Expansion.Contains.Select(x => new CodeDisplay { Code = x.Code, Display = x.Display }).ToList();

                // find root codes in System
                var _roots = CodeSystem.Concept.Where(x => !x.Property.Any(x => x.Code.ToString() == "parent")).ToList();
                foreach (var root in _roots)
                {
                    LoadValues(root, _values);
                }

                LoadMissingDesignations(_codes);
            }

            return _codes;
        }
    }

    private CodeDisplay? LoadValues(CodeSystem.ConceptDefinitionComponent concept, List<CodeDisplay> _values)
    {
        CodeDisplay? disp = null;

        // if the Concept Code is contained in the ValueSet => used it
        if (_values.Any(x => x.Code == concept.Code))
        {
            disp = AddConcept(concept, _values);
        }
        else
        {
            // check if a direct child is in ValueSet, if so, use this code
            foreach (var childCode in concept.Property.Where(x => x.Code == "child").Select(x => x.Value.ToString()))
            {
                if (disp is null)
                {
                    // make sure, child is also valid in CodeSystem and add parent if any child in ValueSet
                    var child = CodeSystem.Concept.FirstOrDefault(x => x.Code == childCode);
                    if (child != null && _values.Any(x => x.Code == child.Code))
                    {
                        disp = AddConcept(concept, _values);
                    }
                }
            }
        }

        // Add Child Concepts
        foreach (var childCode in concept.Property.Where(x => x.Code == "child").Select(x => x.Value.ToString()))
        {
            // first make sure, child is also valid in CodeSystem
            var child = CodeSystem.Concept.FirstOrDefault(x => x.Code == childCode);

            if (child != null)
            {
                // add childs children
                var childDsp = LoadValues(child, _values);
                if (disp is not null && childDsp is not null)
                {
                    disp.Children ??= new();
                    disp.Children.Add(childDsp);
                }
            }
        }

        // add root items to _codes
        if (disp is not null && disp.Parent is null)
        {
            _codes.Add(disp);
        }

        return disp;
    }

    private CodeDisplay AddConcept(CodeSystem.ConceptDefinitionComponent concept, List<CodeDisplay> _values)
    {
        CodeDisplay? item = _values.FirstOrDefault(x => x.Code == concept.Code);
        if (item is null)
        {
            item = new CodeDisplay { Code = concept.Code, Display = concept.Display };
        }

        if (!_values.Any(x => x.Code == item.Code))
        {
            _values.Add(item);
        }
        else
        {
            // Mark Item that it was included in the ValueSet
            item.InValueSet = true;
        }


        // parent
        var parentCode = concept.Property.FirstOrDefault(x => x.Code == "parent")?.Value?.ToString();
        if (parentCode is not null)
        {
            item.Parent = _values.FirstOrDefault(x => x.Code == parentCode);
        }

        return item;
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


[DebuggerDisplay("{Code} - {Display}")]
public class CodeDisplay
{
    public string Code { get; set; }
    public string Display { get; set; }
    public bool InValueSet { get; set; }

    public CodeDisplay? Parent { get; set; }
    public List<CodeDisplay>? Children { get; set; }

    public override string ToString() => $"{Code} - {Display}" + (InValueSet ? "" : " *");
}