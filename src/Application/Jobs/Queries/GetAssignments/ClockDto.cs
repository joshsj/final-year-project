using RendezVous.Application.Common.Dtos;

namespace RendezVous.Application.Jobs.Queries.GetAssignments;

public class ClockDto : EntityDto
{
    public DateTime At { get; set; }
    public Guid? ParentId { get; set; }
}
