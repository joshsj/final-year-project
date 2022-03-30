using RendezVous.Application.Common.Interfaces;

namespace RendezVous.Infrastructure.Services;

public class DateTime : IDateTime
{
    public System.DateTime Now => System.DateTime.UtcNow;
}
