namespace iPath.Domain.Notificxations;

/*
public record NodeNofitication(
    NodeEvent Event,
    eNodeEventType type);
*/

public enum eNodeEventType
{
    None = 0,
    NodeCreated = 1,
    NodePublished = 2,
    NewAttachment = 3,
    NodeDeleted = 4,
    NewAnnotation = 10,
    AnnotationDeleted = 11,
    Test = 99
}