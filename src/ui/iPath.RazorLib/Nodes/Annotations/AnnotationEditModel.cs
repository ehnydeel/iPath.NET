namespace iPath.Blazor.Componenents.Nodes;

public class AnnotationEditModel
{
    public bool AskMorphology { get; set; }

    public AnnotationData Data { get; set; } = new();

    public Guid RootNodeId { get; set; }
    public Guid? ChildNodeId { get; set; }

}
           