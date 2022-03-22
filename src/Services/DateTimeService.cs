using System;
using RendezVous.Services.Common.Interfaces;

namespace RendezVous.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime Now() => DateTime.UtcNow;
}