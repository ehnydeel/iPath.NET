namespace iPath.LHCForms;

public interface IQuestionnaireForm
{
    Task LoadFormAsync(string questionnaire, string questionnaireResponse = "");

    Task<string?> GetDataAsync();
}
