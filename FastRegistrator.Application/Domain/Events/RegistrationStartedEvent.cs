using FastRegistrator.ApplicationCore.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastRegistrator.ApplicationCore.Domain.Events
{
    public record RegistrationStartedEvent(Registration Registration) : INotification;
}
