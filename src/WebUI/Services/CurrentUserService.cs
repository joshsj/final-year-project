using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

using RendezVous.Application.Common.Interfaces;

namespace RendezVous.WebUI.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRendezVousDbContext _rendezVousDbContext;

    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor,
        IRendezVousDbContext rendezVousDbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _rendezVousDbContext = rendezVousDbContext;
    }

    public string? GetProviderId()
    {
        return _httpContextAccessor
            .HttpContext
            ?.User
            ?.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public async Task<Guid?> GetUserId()
    {
        return (await _rendezVousDbContext
            .Employees
            .FirstOrDefaultAsync(e => e.ProviderId == GetProviderId()))
            ?.Id;
    }
}
