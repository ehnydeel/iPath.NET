namespace iPath.Domain.Entities.Base;

public class ConceptFilter
{
    public  string CodeSystem { get; set; }
    public bool IncludingChildCodes { get; set; } = true;
    public List<string> Codes { get; set; } = new();
}
