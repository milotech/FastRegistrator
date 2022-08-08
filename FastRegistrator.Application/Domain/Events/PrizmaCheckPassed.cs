using FastRegistrator.ApplicationCore.Domain.Entities;
using MediatR;

namespace FastRegistrator.ApplicationCore.Domain.Events
{
    public record PrizmaCheckPassedEvent(Registration Registration) : INotification;
}
