namespace RendezVous.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? GetProviderId();
    Task<Guid?> GetUserId();
}
