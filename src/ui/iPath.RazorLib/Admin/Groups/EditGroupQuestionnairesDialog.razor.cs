using Microsoft.Extensions.Localization;
using System.Security.Cryptography;

namespace iPath.Blazor.Componenents.Admin.Groups;

public partial class EditGroupQuestionnairesDialog(IStringLocalizer T)
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; }

    [Parameter]
    public GroupDto Model { get; set; }

    string ErrorMessage = "";
    QuestionnaireListDto selectedQ;


    List<GroupQuestModel> Items = new();


    protected override async Task OnParametersSetAsync()
    {
        foreach(var q in Model.Questionnaires)
        {
            var item = Items.FirstOrDefault(x => x.Id == q.QuestionnaireId);
            if (item is null)
            {
                item = new GroupQuestModel(q.QuestionnaireId, q.QuestinnaireName);
            }
            item.Usage[q.Usage] = true;
        }
    }

    void OnQuestionaireSelected()
    {
        if (selectedQ is not null)
        {
            var item = Items.FirstOrDefault(x => x.Id == selectedQ.Id);
            if (item is null)
            {
                item = new GroupQuestModel(selectedQ.Id, selectedQ.QuestionnaireId);
                Items.Add(item);
            }
            selectedQ = null;
            StateHasChanged();
        }
    }

    private void Cancel() => MudDialog.Cancel();

    private async Task Save()
    {
        StateHasChanged();

    }

    public static List<eQuestionnaireUsage> Usages
    {
        get
        {
            if (field is null)
            {
                field = new List<eQuestionnaireUsage>();
                foreach (var e in Enum.GetValues(typeof(eQuestionnaireUsage)))
                {
                    if ((eQuestionnaireUsage)e != eQuestionnaireUsage.None)
                    {
                        field.Add((eQuestionnaireUsage)e);
                    }
                }
            }
            return field;
        }
    }

}



internal class GroupQuestModel
{
    public Guid Id { get; init; }
    public string Name { get; init; }

    public Dictionary<eQuestionnaireUsage, bool> Usage = new();

    public GroupQuestModel(Guid Id, string Name)
    {
        Id = Id;
        Name = Name;

        foreach (var e in EditGroupQuestionnairesDialog.Usages)
        {
            Usage.Add((eQuestionnaireUsage)e, false);
        }
    }
}