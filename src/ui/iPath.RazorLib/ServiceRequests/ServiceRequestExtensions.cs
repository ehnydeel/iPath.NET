using System.Text.RegularExpressions;

namespace iPath.Blazor.Componenents.ServiceRequests;

// extends iPath.Application.Features.ServiceRequests.ServiceRequestExtensions

public static class ServiceRequestExtensions
{
    extension(ServiceRequestListDto dto)
    {
        public string IsNewIcon => dto.IsNew ? Icons.Material.TwoTone.NewReleases : string.Empty;
        public string HasNewAnnotationIcon => dto.HasNewAnnotation ? Icons.Material.TwoTone.Comment : string.Empty;
    }


    extension(ServiceRequestDto node)
    {      
        public MarkupString DescriptionMarkup => (MarkupString)node?.DescriptionHtml;

        public bool ContainsDocumentId(Guid Id) => node.Documents.Any(c => c.Id == Id);

        public string GetDocumentCaption(Guid Id) => node.Documents.FirstOrDefault(c => c.Id == Id)?.GalleryCaption;
    }


    public static string MakeLink(string txt)
    {
        txt = Regex.Replace(txt,
                @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)",
                "<a target='_blank' href='Guid'>Guid</a>");
        return txt;
    }
}
