using Hl7.Fhir.Model;
using Microsoft.Extensions.DependencyInjection;

namespace iPath.Blazor.Componenents.Questionaires;

public static class QuestionnaireExtension
{
    private static CodingService _coding;

    public static void Initialize(CodingService coding)
    {
        _coding = coding;
    }

    extension(IEnumerable<QuestionnaireForGroupDto> list)
    {
        public async Task<IReadOnlyCollection<QuestionnaireForGroupDto>> FilterAsync(eQuestionnaireUsage qUsage, string? BodySiteCode)
        {
            try
            {
                await _coding.LoadCodeSystem();

                var allForms = list.Where(q => q.Usage == qUsage);
                return allForms.Where(q => _coding.InConceptFilter(BodySiteCode, q.Settings?.BodySiteFilter)).ToList();
            }
            catch(Exception ex)
            {
                Console.WriteLine("QuestionnaireExtension.FilterAsync() exception: " + ex.Message);
            }

            return new List<QuestionnaireForGroupDto>();
        }
    }
}