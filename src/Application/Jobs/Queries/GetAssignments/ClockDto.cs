using RendezVous.Application.Common.Dtos;
using RendezVous.Domain.Enums;

namespace RendezVous.Application.Jobs.Queries.GetAssignments;

public class ClockDto : EntityDto
{
    public ClockType Type { get; set; }
    public DateTime At { get; set; }
    public ClockDto? Parent { get; set; }
}
