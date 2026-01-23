using System.ComponentModel.DataAnnotations;

namespace iPath.Blazor.Componenents.Admin.Questionnaires;

public class QuestionnaireAdminViewModel(ISnackbar snackbar, IDialogService dialog, IPathApi api, IStringLocalizer T, NavigationManager nm)
    : IViewModel
{
    public MudDataGrid<QuestionnaireListDto> grid;
    public iPath.LHCForms.LhcForm preview;


    public List<BreadcrumbItem> BreadCrumbs
    {
        get
        {
            var ret = new List<BreadcrumbItem> { new(T["Administration"], href: "admin") };
            if (SelectedQuestionnaire is null)
            {
                ret.Add(new(T["Questionnaires"], href: null, disabled: true));
            }
            else
            {
                ret.Add(new(T["Questionnaires"], href: "admin/questionnaires"));
                ret.Add(new(SelectedQuestionnaire.Name, href: null, disabled: true));
            }
            return ret;
        }
    }


    public async Task<GridData<QuestionnaireListDto>> GetData(GridState<QuestionnaireListDto> state, CancellationToken ct = default)
    {
        var query = state.BuildQuery(new GetQuestionnaireListQuery());
        var resp = await api.GetQuestionnnaires(query);
        if (resp.IsSuccessful) return resp.Content.ToGridData();
        snackbar.AddError(resp.ErrorMessage);
        return new GridData<QuestionnaireListDto>();
    }


    public async Task Create()
    {
        var dlg = await dialog.ShowAsync<DlgEditQuestionnaire>();
        var res = await dlg.Result;
        if (res?.Data is EditQuestionnaireModel)
        {
            var m = (EditQuestionnaireModel)res.Data;
            var resp = await api.CreateQuestionnaire(new UpdateQuestionnaireCommand(m.QuestionnaireId, m.Name, m.Resource, insert: true));
            if (resp.IsSuccessful)
            {
                await grid.ReloadServerData();
            }
            else
            {
                snackbar.AddError(resp.ErrorMessage);
            }
        }
    }


    public QuestionnaireEntity? SelectedQuestionnaire;
    public async Task Load(Guid Id)
    {
        var resp = await api.GetQuestionnaireById(Id);
        if (snackbar.CheckSuccess(resp))
        {
            SelectedQuestionnaire = resp.Content;
        }
    }


    public async Task Edit(QuestionnaireListDto item)
    {
        if (item != null)
        {
            var resp = await api.GetQuestionnaireById(item.Id);
            if (resp.IsSuccessful)
            {
                var v1 = resp.Content.Resource;
                var m = new EditQuestionnaireModel
                {
                    Id = resp.Content.Id,
                    QuestionnaireId = resp.Content.QuestionnaireId,
                    Name = resp.Content.Name,
                    Version = resp.Content.Version,
                    Resource = resp.Content.Resource
                };
                var p = new DialogParameters<DlgEditQuestionnaire> { { x => x.Model, m } };
                var dlg = await dialog.ShowAsync<DlgEditQuestionnaire>("...", parameters: p);
                var res = await dlg.Result;
                if (res?.Data is EditQuestionnaireModel)
                {
                    var r = (EditQuestionnaireModel)res.Data;

                    if (v1 != r.Resource)
                    {
                        var resp2 = await api.CreateQuestionnaire(new UpdateQuestionnaireCommand(r.QuestionnaireId, r.Name, r.Resource, insert: false));
                        await grid.ReloadServerData();
                        snackbar.Add("Questionnaire updated", Severity.Success);
                    }
                    else
                    {
                        snackbar.Add("no change", Severity.Info);
                    }
                }
            }
        }
    }

    public async Task Delete(QuestionnaireListDto item)
    {
        snackbar.Add("not implemented yet", Severity.Info);
    }
}

public class EditQuestionnaireModel
{
    public Guid? Id { get; init; }
    public int Version { get; init; }

    [Required]
    public string QuestionnaireId { get; set; }

    [Required]
    public string Name { get; set; }


    public IBrowserFile? ResourceFile { get; set; }
    public string ResourceFileName { get; set; }

    public string? Resource { get; set; }
}
