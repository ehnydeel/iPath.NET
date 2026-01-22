using System.Diagnostics;

namespace iPath.Application.Coding;

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