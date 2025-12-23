namespace iPath.Domain.Entities;

public class NodeDescription
{
    public string? Subtitle { get; set; }
    public string? CaseType { get; set; }
    public string? AccessionNo { get; set; }
    public string? Status { get; set; }

    [Required, MinLength(3)]
    public string? Title { get; set; } = string.Empty!;
    public string? Text { get; set; } = string.Empty!;

    public CodedConcept BodySite { get; set; }

    public NodeDescription Clone() => (NodeDescription)MemberwiseClone();
}


public class CodedConcept
{
    public string System { get; set; }
    public string Code { get; set; }
    public string Display { get; set; }

    public override string ToString() => $"{Display} [{Code}]";
}