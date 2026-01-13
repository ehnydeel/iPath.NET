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

    public eNodeVisibility Visibility { get; set; } = eNodeVisibility.GroupMembers;

    public NodeDescription? Description { get; set; } = new();

    public NodeFile? File { get; set; } = null!;

    public ICollection<QuestionnaireResponseEntity> QuestionnaireResponses { get; set; } = [];

}


public enum eNodeVisibility
{
    GroupMembers = 0,
    Private = 1,
    Public = 2
}
