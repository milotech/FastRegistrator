using FastRegistrator.Application.Domain.Entities;
using MediatR;

namespace FastRegistrator.Application.Domain.Events
{
    public record RegistrationCompletedEvent(Registration Registration) : INotification;
}
