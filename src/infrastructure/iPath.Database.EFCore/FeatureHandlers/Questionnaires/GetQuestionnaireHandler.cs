
namespace iPath.EF.Core.FeatureHandlers.Questionnaires;

public class GetQuestionnaireHandler(iPathDbContext db)
     : IRequestHandler<GetQuestionnaireQuery, Task<QuestionnaireEntity>>
{
    public async Task<QuestionnaireEntity> Handle(GetQuestionnaireQuery request, CancellationToken cancellationToken)
    {
        var q = db.Questionnaires.AsNoTracking()
            .Where(q => q.QuestionnaireId == request.QuestionnaireId);

        if (request.Version.HasValue)
        {
            q = q.Where(q => q.Version == request.Version.Value);
        }
        else
        {
            q = q.Where(q => q.IsActive);
        }

        return await q.FirstOrDefaultAsync(cancellationToken);
    }
}