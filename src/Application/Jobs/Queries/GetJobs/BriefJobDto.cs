using RendezVous.Application.Common.Dtos;
using RendezVous.Domain.Models;

namespace RendezVous.Application.Jobs.Queries.GetJobs;

public class BriefJobDto : EntityDto
{
    public string Title { get; set; } = null!;
    
}
