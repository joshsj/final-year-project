using RendezVous.Domain.Common;

namespace RendezVous.Application.Common.Interfaces;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}
