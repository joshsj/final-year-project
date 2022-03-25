using RendezVous.Application.Common.Interfaces;

namespace RendezVous.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
}
