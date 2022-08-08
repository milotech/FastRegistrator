using FastRegistrator.ApplicationCore.Domain.Entities;
using MediatR;

namespace FastRegistrator.ApplicationCore.Domain.Events
{
    public record RegistrationCompletedEvent(Registration Registration) : INotification;
}
