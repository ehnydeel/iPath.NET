namespace iPath.Domain.Entities;

public class RequestDescription
{
    public string? Subtitle { get; set; }
    public string? CaseType { get; set; }
    public string? AccessionNo { get; set; }
    public string? Status { get; set; }

    [Required, MinLength(3)]
    public string? Title { get; set; } = string.Empty!;
    public string? Text { get; set; } = string.Empty!;

    public QuestionnaireResponseData? Questionnaire { get; set; }

    public CodedConcept? BodySite { get; set; }

    public RequestDescription Clone() => (RequestDescription)MemberwiseClone();
}

