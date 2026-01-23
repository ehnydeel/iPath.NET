using Hl7.Fhir.Model;
using iPath.Application.Coding;
using System.Diagnostics;

namespace iPath.Application.Coding;

public class CodeSystemLookup
{
    private readonly Dictionary<string, List<string>> _childrenLookup;
    private readonly Dictionary<string, List<string>> _parentLookup;
    private readonly IEqualityComparer<string> _comparer;

    public CodeSystemLookup(CodeSystem codeSystem)
    {
        _comparer = (codeSystem?.CaseSensitive == true) ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;
        _childrenLookup = new Dictionary<string, List<string>>(_comparer);
        _parentLookup = new Dictionary<string, List<string>>(_comparer);

        if (codeSystem?.Concept != null)
            Build(codeSystem);
    }

    public bool IsEmpty => _childrenLookup.Count == 0;

    public CodeLookupResult GetChildCodes(string root, bool includeRoot = true)
    {
        if (IsEmpty)
            return CodeLookupResult.WithError("codesystem is empty");
        if (string.IsNullOrEmpty(root))
            return CodeLookupResult.WithError("no root code");

        var sw = Stopwatch.StartNew();
        TimeSpan elapsed;
        var result = new HashSet<string>(_comparer);

        try
        {
            var visited = new HashSet<string>(_comparer);
            var stack = new Stack<string>();

            if (includeRoot && !result.Contains(root))
                result.Add(root);

            if (_childrenLookup.TryGetValue(root, out var direct))
            {
                foreach (var c in direct)
                    stack.Push(c);

                while (stack.Count > 0)
                {
                    var code = stack.Pop();
                    if (!visited.Add(code)) continue;
                    result.Add(code);

                    if (_childrenLookup.TryGetValue(code, out var children) && children.Count > 0)
                    {
                        foreach (var child in children)
                            if (!visited.Contains(child))
                                stack.Push(child);
                    }
                }
            }
            else
            {
                // root present but has no children -> return (maybe includeRoot is true)
            }
        }
        catch (Exception ex)
        {
            sw.Stop();
            return new CodeLookupResult(root, new HashSet<string>(_comparer), sw.Elapsed, ex.Message);
        }
        finally
        {
            sw.Stop();
            elapsed = sw.Elapsed;
        }

        return new CodeLookupResult(root, result, elapsed);
    }

    // More efficient IsChildCode: traverse upwards from child to root using parent lookup.
    // Stops early when root is found and avoids building the full descendant set.
    public bool IsChildCode(string childCode, string rootCode, bool includeRoot = true)
    {
        if (string.IsNullOrEmpty(childCode) || string.IsNullOrEmpty(rootCode)) return false;
        if (IsEmpty) return false;

        // If includeRoot and codes are equal, treat as child
        if (includeRoot && _comparer.Equals(childCode, rootCode)) return true;

        // If includeRoot is false and codes are equal, it's not considered a child.
        if (!_comparer.Equals(childCode, rootCode))
        {
            // BFS/DFS up the parent graph from the child until we either find the root or exhaust ancestors.
            var visited = new HashSet<string>(_comparer);
            var stack = new Stack<string>();
            stack.Push(childCode);

            while (stack.Count > 0)
            {
                var cur = stack.Pop();
                if (!visited.Add(cur)) continue;

                if (!_parentLookup.TryGetValue(cur, out var parents) || parents.Count == 0)
                    continue;

                foreach (var p in parents)
                {
                    if (_comparer.Equals(p, rootCode))
                        return true;

                    if (!visited.Contains(p))
                        stack.Push(p);
                }
            }
        }

        return false;
    }

    private void Build(CodeSystem codeSystem)
    {
        // local helper to add mapping parent -> child AND child -> parent
        void AddChild(string parent, string child)
        {
            if (string.IsNullOrEmpty(parent) || string.IsNullOrEmpty(child)) return;

            if (!_childrenLookup.TryGetValue(parent, out var list))
            {
                list = new List<string>();
                _childrenLookup[parent] = list;
            }

            if (!list.Contains(child, _comparer))
                list.Add(child);

            if (!_childrenLookup.ContainsKey(child))
                _childrenLookup[child] = new List<string>();

            // parent lookup
            if (!_parentLookup.TryGetValue(child, out var plist))
            {
                plist = new List<string>();
                _parentLookup[child] = plist;
            }

            if (!plist.Contains(parent, _comparer))
                plist.Add(parent);

            // ensure parent also has an entry in parent lookup (may be leaf-less)
            if (!_parentLookup.ContainsKey(parent))
                _parentLookup[parent] = new List<string>();
        }

        // extract simple string value from concept property value
        static string? GetPropertyValue(CodeSystem.ConceptPropertyComponent prop)
        {
            if (prop == null) return null;
            if (prop.Value is Code codeVal) return codeVal.Value;
            if (prop.Value is FhirString fs) return fs.Value;
            var s = prop.Value?.ToString();
            return string.IsNullOrWhiteSpace(s) ? null : s;
        }

        // recursively process nested concept trees and property-defined relations
        void AddConceptRecursive(string? parentCode, CodeSystem.ConceptDefinitionComponent concept)
        {
            if (concept == null) return;
            var code = concept.Code;
            if (string.IsNullOrEmpty(code)) return;

            if (!_childrenLookup.ContainsKey(code))
                _childrenLookup[code] = new List<string>();

            if (!string.IsNullOrEmpty(parentCode))
                AddChild(parentCode, code);

            if (concept.Concept != null && concept.Concept.Count > 0)
            {
                foreach (var child in concept.Concept)
                    AddConceptRecursive(code, child);
            }

            if (concept.Property != null)
            {
                foreach (var prop in concept.Property)
                {
                    if (string.Equals(prop.Code, "child", StringComparison.OrdinalIgnoreCase))
                    {
                        var childCode = GetPropertyValue(prop);
                        if (!string.IsNullOrEmpty(childCode))
                            AddChild(code, childCode);
                    }
                    else if (string.Equals(prop.Code, "parent", StringComparison.OrdinalIgnoreCase))
                    {
                        var parent = GetPropertyValue(prop);
                        if (!string.IsNullOrEmpty(parent))
                            AddChild(parent, code);
                    }
                }
            }
        }

        foreach (var top in codeSystem.Concept)
        {
            AddConceptRecursive(null, top);
        }
    }
}