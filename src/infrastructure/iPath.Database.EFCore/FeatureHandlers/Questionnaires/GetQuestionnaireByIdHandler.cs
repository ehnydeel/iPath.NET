
namespace iPath.EF.Core.FeatureHandlers.Questionnaires;

public class GetQuestionnaireByIdHandler(iPathDbContext db)
     : IRequestHandler<GetQuestionnaireByIdQuery, Task<QuestionnaireEntity>>
{
    public async Task<QuestionnaireEntity> Handle(GetQuestionnaireByIdQuery request, CancellationToken cancellationToken)
    {
        return await db.Questionnaires.AsNoTracking().FirstOrDefaultAsync(q => q.Id == request.Id);
    }
}