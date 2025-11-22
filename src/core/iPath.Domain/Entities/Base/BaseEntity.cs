namespace iPath.Domain.Entities;


public interface IEntity
{
}

public interface IBaseEntity
{
    Guid Id { get; set; }
}

public class BaseEntity : IBaseEntity
{
    public Guid Id { get; set; }
}

public class AuditableEntity : BaseEntity
{
    public DateTime? DeletedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}



public class AuditableEntityWithEvents : AuditableEntity, IHasDomainEvents
{
    public DateTime? DeletedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }

    [JsonIgnore]
    public List<EventEntity> Events => new();

    public void AddDomainEvent(EventEntity eventItem) => Events.Add(eventItem);

    public void ClearDomainEvents() => Events.Clear();
}


public interface IHasDomainEvents : IBaseEntity
{
    public List<EventEntity> Events { get; }
    public void AddDomainEvent(EventEntity eventItem);
    public void ClearDomainEvents();
}