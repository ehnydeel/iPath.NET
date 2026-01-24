using Hl7.Fhir.Model;
using iPath.Application.Coding;

namespace iPath.Application.Coding;

public class ValueSetDisplay
{
    private readonly ValueSet? _valueSet;

    public ValueSetDisplay(CodeSystem codeSystem, ValueSet valueSet)
    {
        _valueSet = valueSet;
        CodeSystem = codeSystem;
        LoadData();
    }

    public ValueSet? ValueSet => _valueSet;

    private string? CodeSystemUrl => CodeSystem.Url;
    public readonly CodeSystem CodeSystem;


    private List<CodeDisplay> _displayTree;
    private List<CodeDisplay> _values;

    public List<CodeDisplay> DisplayTree => _displayTree;

    private void LoadData()
    {
        if (_displayTree is null)
        {
            _displayTree = new List<CodeDisplay>();

            _values = ValueSet.Expansion.Contains.Select(x => x.ToDisplay()).ToList();

            // find root codes in System
            var _roots = CodeSystem.Concept.Where(x => !x.Property.Any(x => x.Code.ToString() == "parent")).ToList();
            foreach (var root in _roots)
            {
                LoadValues(root, _values);
            }
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
            _displayTree.Add(disp);
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


    public async Task<IEnumerable<CodeDisplay>> FindConcepts(string search, CancellationToken ct = default)
    {
        return _values.FindConcepts(search);
    }


    public CodeDisplay GetByCode(string code)
    {
        return _values.FirstOrDefault(x => x.Code == code);
    }
}
