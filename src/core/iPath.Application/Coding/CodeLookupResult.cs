using System.Collections.Generic;

namespace iPath.Application.Coding;

public record CodeLookupResult(string RootCode, HashSet<string> ChildCodes, TimeSpan Elapsed, string? Error = null)
{
    public static CodeLookupResult WithError(string msg) => new CodeLookupResult("", [], TimeSpan.FromSeconds(0), msg);
    public bool IsSuccess => Error is null;
}
