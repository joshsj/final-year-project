using RendezVous.Application.Common.Dtos;

namespace RendezVous.Application.Jobs.Queries.GetJobs;

public class BriefJobDto : EntityDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public string LocationTitle { get; set; } = null!;
    public int AssignmentCount { get; set; }
}
