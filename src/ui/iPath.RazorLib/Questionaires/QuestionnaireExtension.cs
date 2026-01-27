using Hl7.Fhir.Model;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace iPath.Blazor.Componenents.Questionaires;

public static class QuestionnaireExtension
{
    private static IServiceProvider _sp;

    public static void Initialize(IServiceProvider serviceProvider)
    {        _sp = serviceProvider;
        
    }

    extension(IEnumerable<QuestionnaireForGroupDto> list)
    {
        public async Task<IReadOnlyCollection<QuestionnaireForGroupDto>> FilterAsync(eQuestionnaireUsage qUsage, string? BodySiteCode)
        {
            try
            {
                var coding = _sp.GetKeyedService<CodingService>("icdo");

                await coding.LoadCodeSystem();

                var allForms = list.Where(q => q.Usage == qUsage);
                return allForms.Where(q => coding.InConceptFilter(BodySiteCode, q.Settings?.BodySiteFilter)).ToList();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new List<QuestionnaireForGroupDto>();
        }
    }
}