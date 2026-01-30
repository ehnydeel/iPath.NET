namespace iPath.EF.Core.FeatureHandlers.ServiceRequests.Queries;

public static class ServiceRequestQueryExtensions
{
    extension(IQueryable<ServiceRequest> q)
    {
        public IQueryable<ServiceRequestListDto> ProjectToList()
        {
            return q.Select(n => new ServiceRequestListDto
            {
                Id = n.Id,
                NodeType = n.NodeType,
                CreatedOn = n.CreatedOn,
                IsDraft = n.IsDraft,
                OwnerId = n.OwnerId,
                Owner = new OwnerDto(n.OwnerId, n.Owner.UserName, n.Owner.Email),
                GroupId = n.GroupId,
                Description = n.Description
            });
        }

        public IQueryable<ServiceRequestListDto> ProjectToListDetails(Guid uid)
        {
            return q.Select(n => new ServiceRequestListDto
            {
                Id = n.Id,
                NodeType = n.NodeType,
                CreatedOn = n.CreatedOn,
                IsDraft = n.IsDraft,
                OwnerId = n.OwnerId,
                Owner = new OwnerDto(n.OwnerId, n.Owner.UserName, n.Owner.Email),
                GroupId = n.GroupId,
                Description = n.Description,
                AnnotationCount = n.Annotations.Count(),
                LastAnnotationDate = n.Annotations.Max(a => a.CreatedOn),
                LastVisit = n.LastVisits.Where(v => v.UserId == uid).Max(v => v.Date)
            });
        }
    }
}
