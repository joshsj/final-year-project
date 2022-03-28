using RendezVous.Application.Common.Interfaces;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace RendezVous.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly ICurrentUserService _currentUserService;
    private readonly IRendezVousDbContext _rendezVousDbContext;

    public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService, IRendezVousDbContext rendezVousDbContext)
    {
        _logger = logger;
        _currentUserService = currentUserService;
        _rendezVousDbContext = rendezVousDbContext;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var currentUser = (await _rendezVousDbContext
            .Employees
            .FirstOrDefaultAsync(x => x.ProviderId == _currentUserService.ProviderId, cancellationToken));

        _logger.LogInformation("RendezVous Request: {Name} {@UserId} {@Name} {@Request}",
            requestName, currentUser?.Id, currentUser?.Name, request);
    }
}
