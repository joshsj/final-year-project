using System.Diagnostics;
using RendezVous.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace RendezVous.Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly Stopwatch _timer;
    private readonly ILogger<TRequest> _logger;
    private readonly ICurrentUserService _currentUserService;
    private readonly IRendezVousDbContext _rendezVousDbContext;

    public PerformanceBehaviour(
        ILogger<TRequest> logger,
        ICurrentUserService currentUserService,
        IRendezVousDbContext rendezVousDbContext)
    {
        _timer = new Stopwatch();

        _logger = logger;
        _currentUserService = currentUserService;
        _rendezVousDbContext = rendezVousDbContext;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        _timer.Start();

        var response = await next();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            var requestName = typeof(TRequest).Name;
            var currentUser = (await _rendezVousDbContext
                .Employees
                .FirstOrDefaultAsync(x => x.ProviderId == _currentUserService.ProviderId, cancellationToken));

            _logger.LogWarning("RendezVous Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@Name} {@Request}",
                requestName, elapsedMilliseconds, currentUser?.Id, currentUser?.Name, request);
        }

        return response;
    }
}
