using iPath.Application.Features.Documents;

namespace iPath.Blazor.Componenents.Documents;

public static class DocumentExtensions
{

    extension(DocumentDto node)
    {
        public string? Title
        {
            get
            {
                return node.File.Filename;
            }
        }

        public string GalleryCaption => node?.File is null ? "" : node.File.Filename;
               

        public string ThumbUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(node.File?.ThumbData))
                {
                    return $"data:image/jpeg;base64, {node.File.ThumbData}";
                }
                else if (node.ipath2_id.HasValue)
                {
                    return $"https://www.ipath-network.com/ipath/image/src/{node.ipath2_id}";
                }

                return "";
            }
        }

        public string BinarayDataUrl => $"/files/{node.Id}";

        public string FileUrl
        {
            get
            {
                if (!node.ipath2_id.HasValue)
                {
                    return $"/api/v1/nodes/file/{node.Id}/{node.File.Filename}";
                }
                else if (node.ipath2_id.HasValue)
                {
                    return $"https://www.ipath-network.com/ipath/image/src/{node.ipath2_id}";
                }

                return "";
            }
        }

        public bool IsImage => node.NodeType == "image";

        public string FileExtension
        {
            get
            {
                if (node is not null && node.File is not null && !string.IsNullOrEmpty(node.File.Filename))
                {
                    var fi = new FileInfo(node.File.Filename);
                    return fi.Extension;
                }
                return string.Empty;
            }
        }

        public string FileIcon
        {
            get
            {
                if (node.FileExtension == ".pdf")
                    return Icons.Custom.FileFormats.FilePdf;

                if (node.FileExtension == ".svs")
                    return Icons.Custom.FileFormats.FileImage;

                if (node.NodeType.ToLower() == "folder")
                    return Icons.Material.Filled.FolderOpen;

                return Icons.Custom.FileFormats.FileDocument;
            }
        }
    }

}
