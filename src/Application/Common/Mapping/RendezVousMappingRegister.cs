using Mapster;
using RendezVous.Application.Common.Interfaces;
using RendezVous.Application.Jobs.Queries.GetJobs;
using RendezVous.Domain.Entities;

namespace RendezVous.Application.Common.Mapping;

public class RendezVousMappingRegister: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<Job, BriefJobDto>()
            .Map(
                to => to.AssignmentCount,
                from => from.Assignments.Count);
    }
}
