using System.Text.RegularExpressions;

namespace iPath.Application.Features.ServiceRequests;

public static class ServiceRequestExtensions
{
    extension(RequestDescription? dto)
    {
        public string? FullTitle()
        {
            if (dto is null) return "--";

            var parts = new List<string>();
            if (!string.IsNullOrEmpty(dto?.Title))
                parts.Add(dto.Title);
            if (!string.IsNullOrEmpty(dto?.AccessionNo))
                parts.Add(dto.AccessionNo);
            if (!parts.Any())
                parts.Add("(No Title)");

            return string.Join(", ", parts);
        }

        public bool ValidateInput()
        {
            if (dto is null) return false;
            // if any of the main fields are filled, consider it valid
            if (!string.IsNullOrWhiteSpace(dto.Text)) return true;
            if (!string.IsNullOrWhiteSpace(dto.Title)) return true;
            if (!string.IsNullOrWhiteSpace(dto.AccessionNo)) return true;
            if (dto.Questionnaire is not null) return true;
            return false;
        }
    }

    extension(ServiceRequestListDto dto)
    {
        public string? Title
        {
            get
            {
                var ret = dto.Description.FullTitle();
                if (dto.IsDraft)
                    ret += " (Draft)";
                return ret;
            }
        }            
            
            
        public string? SubTitle => dto?.Description?.Subtitle;
        public string? AccessionNo => dto?.Description?.AccessionNo;


        public bool IsNew => dto.LastVisit is null;

        public bool HasNewAnnotation
        {
            get
            {
                if (dto.LastAnnotationDate.HasValue)
                {
                    if (!dto.LastVisit.HasValue)
                        return true;
                    else
                        return dto.LastVisit.Value < dto.LastAnnotationDate.Value;
                }
                return false;
            }
        }
    }




    extension(ServiceRequestDto node)
    {
        public string? Title
        {
            get
            {
                return node?.Description.FullTitle();
            }
        }

        public string? SubTitle => node?.Description?.Subtitle;
        public string? AccessionNo => node?.Description?.AccessionNo;
        public string? CaseType => node?.Description?.CaseType;
        public string? DescriptionHtml
        {
            get
            {
                var html = node?.Description?.Text ?? "";

                // replace line breaks
                html = html.ReplaceLineEndings("<br />\n");

                // replace links
                html = MakeLink(html);

                return html;
            }
        }

        public string? NodeTitle => node?.File is null ? node.Description?.Title : node.File.Filename;
    }


    public static string MakeLink(string txt)
    {
        txt = Regex.Replace(txt,
                @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)",
                "<a target='_blank' href='Guid'>Guid</a>");
        return txt;
    }
}
