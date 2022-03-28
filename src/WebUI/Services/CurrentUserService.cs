using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

using RendezVous.Application.Common.Interfaces;

namespace RendezVous.WebUI.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? ProviderId => 
        _httpContextAccessor
            .HttpContext
            ?.User
            ?.FindFirstValue(ClaimTypes.NameIdentifier);
}
