using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastRegistrator.ApplicationCore.Domain.Events
{
    // TODO: Registration Entity with included PersonData here
    public record RegistrationStartedEvent(Guid RegistrationId) : INotification;
}
