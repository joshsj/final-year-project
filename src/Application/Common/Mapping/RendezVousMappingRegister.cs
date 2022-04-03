using Mapster;
using RendezVous.Application.Jobs.Queries.GetAssignments;
using RendezVous.Application.Jobs.Queries.GetJobs;
using RendezVous.Domain.Entities;
using RendezVous.Domain.Enums;

namespace RendezVous.Application.Common.Mapping;

public class RendezVousMappingRegister: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.RequireExplicitMapping = true;
        // config.RequireDestinationMemberSource = true;
        
        config.ForType<Job, BriefJobDto>()
            .Map(
                to => to.AssignmentCount,
                from => from.Assignments.Count);

        config.ForType<Assignment, AssignmentDto>();
        config.ForType<Clock, ClockDto>();
    }
}
