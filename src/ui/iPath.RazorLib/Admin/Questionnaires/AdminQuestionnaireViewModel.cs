namespace iPath.Blazor.Componenents.Admin.Questionnaires;

public class AdminQuestionnaireViewModel(ISnackbar snackbar, IDialogService dialog, IPathApi api)
    : IViewModel
{
    public MudDataGrid<QuestionnaireListDto> grid;

    public async Task<GridData<QuestionnaireListDto>> GetData(GridState<QuestionnaireListDto> state)
    {
        var query = state.BuildQuery(new GetQuestionnaireListQuery());
        var resp = await api.GetQuestionnnaires(query);
        if (resp.IsSuccessful) return resp.Content.ToGridData();
        snackbar.AddError(resp.ErrorMessage);
        return new GridData<QuestionnaireListDto>();
    }


    public async Task Create()
    {
        var dlg = await dialog.ShowAsync<EditQuestionnaireDialog>();
        var res = await dlg.Result;
        if (res?.Data is EditQuestionnaireModel)
        {
            var m = (EditQuestionnaireModel)res.Data;
            var resp = await api.CreateQuestionnaire(new CreateQuestionnaireCommand(m.QuestionnaireId, m.Resource));
            await grid.ReloadServerData();
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
                    Version = resp.Content.Version,
                    Resource = resp.Content.Resource
                };
                var p = new DialogParameters<EditQuestionnaireDialog> { { x => x.Model, m } };
                var dlg = await dialog.ShowAsync<EditQuestionnaireDialog>("...", parameters: p);
                var res = await dlg.Result;
                if (res?.Data is EditQuestionnaireModel)
                {
                    var r = (EditQuestionnaireModel)res.Data;

                    if (v1 != r.Resource)
                    {
                        var resp2 = await api.CreateQuestionnaire(new CreateQuestionnaireCommand(r.QuestionnaireId, r.Resource));
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
}

public class EditQuestionnaireModel
{
    public Guid? Id { get; init; }
    public int Version { get; init; }
    public string QuestionnaireId { get; set; }
    public string Resource { get; set; }
}
