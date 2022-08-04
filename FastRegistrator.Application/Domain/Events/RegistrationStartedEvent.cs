using FastRegistrator.ApplicationCore.Domain.Entities;
using MediatR;

namespace FastRegistrator.ApplicationCore.Domain.Events
{
    public record RegistrationStartedEvent(Registration Registration) : INotification;
}
