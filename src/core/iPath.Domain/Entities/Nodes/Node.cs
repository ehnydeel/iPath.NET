namespace iPath.Domain.Entities;

public class Node : AuditableEntityWithEvents
{
    public int? ipath2_id { get; set; }

    public string? StorageId { get; set; }

    public DateTime CreatedOn { get; set; }
    public Guid OwnerId { get; set; }
    public User Owner { get; set; } = null!;

    public Guid? GroupId { get; set; }
    public Group? Group { get; set; }

    public Guid? RootNodeId { get; set; }
    [JsonIgnore]
    public Node? RootNode { get; set; }

    public Guid? ParentNodeId { get; set; }

    public int? SortNr { get; set; } = 0;
    public bool IsDraft { get; set; }

    public ICollection<Node> ChildNodes { get; set; } = [];
    public ICollection<Annotation> Annotations { get; set; } = [];


    // public ICollection<FileUpload> Uploads { get; set; } = [];
    public ICollection<NodeLastVisit>? LastVisits { get; set; } = [];

    public string NodeType { get; set; } = default!;

    public NodeDescription? Description { get; set; } = new();

    public NodeFile? File { get; set; } = null!;

    public ICollection<QuestionnaireResponse> QuestionnaireResponses { get; set; } = [];

}

public class NodeFile
{
    public DateTime? LastStorageExportDate { get; set; }
    public string? Filename { get; set; }
    public string? MimeType { get; set; }
    public string? ThumbData { get; set; }
    public int? ImageWidth { get; set; }
    public int? ImageHeight { get; set; }

    public NodeFile Clone() => (NodeFile)MemberwiseClone();
}