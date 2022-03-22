using System;
using RendezVouz.Services.Common.Interfaces;

namespace RendezVouz.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime Now() => DateTime.UtcNow;
}