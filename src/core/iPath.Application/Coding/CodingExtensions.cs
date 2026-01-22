using Hl7.Fhir.Model;
using iPath.Application.Coding;


namespace iPath.Application.Coding;

public static class CodingExtensions
{
    extension(string display)
    {
        public bool ContainsWordsStartingWith(string term)
        {
            if (string.IsNullOrWhiteSpace(term) || string.IsNullOrEmpty(display)) return false;

            var terms = term.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            bool hasUnmatchedTerm = false;

            foreach (var t in terms)
            {
                bool termMatched = false;
                var wordsInDisplay = display.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (var word in wordsInDisplay)
                {
                    var w = word.Replace("[", "");
                    w = w.Replace("(", "");

                    if (w.StartsWith(t, System.StringComparison.OrdinalIgnoreCase))
                        termMatched = true;
                }
                if (!termMatched)
                {
                    hasUnmatchedTerm = true;
                    break;
                }
            }

            return !hasUnmatchedTerm;
        }
    }

    extension(ValueSet.ContainsComponent code)
    {
        public CodeDisplay ToDisplay()
            => new CodeDisplay { Code = code.Code, Display = code.Display };

        public bool ContainsWordsStartingWith(string term)
        {
            // compare Display
            if (code.Display.ContainsWordsStartingWith(term)) return true;

            // Fallback to code equality
            return (string.Compare(code.Code, term, true) == 0);
        }
    }


    extension(CodeSystem.ConceptDefinitionComponent code)
    {
        public CodeDisplay ToDisplay()
            => new CodeDisplay { Code = code.Code, Display = code.Display };

        public bool ContainsWordsStartingWith(string term)
        {
            // compare Display
            if (code.Display.ContainsWordsStartingWith(term)) return true;

            // Fallback to code equality
            return (string.Compare(code.Code, term, true) == 0);
        }
    }

    extension(CodeDisplay dsp)
    {
        public string ToDisplay()
            => dsp is null ? "" : $"{dsp.Display} [{dsp.Code}]";

        public string ToAppend()
            => dsp is null ? "" : $"- {dsp.Display} [{dsp.Code}]";


        public IEnumerable<CodeDisplay> ToFlatChildList()
        {
            var ret = new List<CodeDisplay>();
            if (dsp is not null && dsp.Children is not null && dsp.Children.Any())
            {
                foreach (var child in dsp.Children) 
                {
                    ret.Add(child);
                    ret.AddRange(child.ToFlatChildList());
                }
            }
            return ret;
        }

        public bool ContainsWordsStartingWith(string term)
        {
            // compare Display
            if (dsp.Display.ContainsWordsStartingWith(term)) return true;

            // Fallback to code equality
            return (string.Compare(dsp.Code, term, true) == 0);
        }
    }


    extension(IEnumerable<CodeDisplay>? values)
    {
        public IEnumerable<CodeDisplay> FindConcepts(string search, CancellationToken ct = default)
        {
            if (values is null )
            {
                return new List<CodeDisplay>();
            } 
            else if (string.IsNullOrEmpty(search))
            {
                // show concepts in valueset that have a display string
                return values.Where(x => x.InValueSet && !string.IsNullOrEmpty(x.Display))
                    .OrderBy(x => x.Display);
            }
            else
            {
                search = search.ToLower();
                return values
                    .Where(x => x.InValueSet && x.ContainsWordsStartingWith(search))
                    .OrderBy(x => x.Display)
                    .ToArray();
            }
        }
    }

}