using FluentValidation;
using FluentValidation.Results;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RendezVous.Application.Common.Extensions;
using RendezVous.Application.Common.Interfaces;
using RendezVous.Domain.Entities;

namespace RendezVous.Application.Jobs.Queries.GetAssignments;

public class GetAssignmentsQuery : IRequest<IList<AssignmentDto>>
{
    public Guid JobId { get; set; }
}

public class GetAssignmentsQueryValidator : AbstractValidator<GetAssignmentsQuery>
{
    private readonly IRendezVousDbContext _dbContext;

    public GetAssignmentsQueryValidator(IRendezVousDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.JobId)
            .NotNull()
            .NotEqual(Guid.Empty);
    }

    public override async Task<ValidationResult> ValidateAsync(
        ValidationContext<GetAssignmentsQuery> context,
        CancellationToken ct = new())
    {
        var result = await base.ValidateAsync(context, ct);

        if (!result.IsValid) { return result; }

        var job = await _dbContext.Jobs.SingleOrDefaultAsync(
            x => x.Id == context.InstanceToValidate.JobId,
            ct);

        if (job is null)
        {
            context.AddMissingEntityFailure(nameof(Job), context.InstanceToValidate.JobId);
        }

        return result;
    }
}

public class GetAssignmentsQueryHandler : IRequestHandler<GetAssignmentsQuery, IList<AssignmentDto>>
{
    private readonly IRendezVousDbContext _dbContext;
    private readonly TypeAdapterConfig _typeAdapterConfig;

    public GetAssignmentsQueryHandler(
        IRendezVousDbContext dbContext,
        TypeAdapterConfig typeAdapterConfig)
    {
        _dbContext = dbContext;
        _typeAdapterConfig = typeAdapterConfig;
    }

    public async Task<IList<AssignmentDto>> Handle(GetAssignmentsQuery request, CancellationToken ct)
    {
        return await _dbContext.Assignments.AsNoTracking()
            .Where(x => x.JobId == request.JobId)
            .Include(x => x.Employee)
            .Include(x => x.Clocks)
            .ProjectToType<AssignmentDto>(_typeAdapterConfig)
            .ToListAsync(ct);
    }
}
