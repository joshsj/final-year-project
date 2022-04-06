using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RendezVous.Application.Common.Interfaces;

namespace RendezVous.Application.Jobs.Queries.GetJobs;

public class GetJobsQuery : IRequest<IList<BriefJobDto>> { }

public class GetJobsQueryHandler : IRequestHandler<GetJobsQuery, IList<BriefJobDto>>
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
    
    public async Task<IList<BriefJobDto>> Handle(GetJobsQuery request, CancellationToken ct)
    {
        return await _dbContext.Jobs.AsNoTracking()
            .ProjectToType<BriefJobDto>(_typeAdapterConfig)
            .ToListAsync(ct);
    }
}
