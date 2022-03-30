using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RendezVous.Application.Common.Interfaces;

namespace RendezVous.Application.Jobs.Queries.GetJobs;

public class GetJobsQuery : IRequest<IEnumerable<BriefJobDto>> { }

public class GetJobsQueryHandler : IRequestHandler<GetJobsQuery, IEnumerable<BriefJobDto>>
{
    private readonly IRendezVousDbContext _dbContext;
    private readonly TypeAdapterConfig _typeAdapterConfig;

    public GetJobsQueryHandler(
        IRendezVousDbContext dbContext,
        TypeAdapterConfig typeAdapterConfig)
    {
        _dbContext = dbContext;
        _typeAdapterConfig = typeAdapterConfig;
    }
    
    public Task<IEnumerable<BriefJobDto>> Handle(GetJobsQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(
            _dbContext.Jobs.AsNoTracking()
                .ProjectToType<BriefJobDto>(_typeAdapterConfig)
                .AsEnumerable());
    }
}
