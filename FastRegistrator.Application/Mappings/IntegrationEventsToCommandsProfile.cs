using AutoMapper;
using FastRegistrator.Application.Commands.StartRegistration;
using FastRegistrator.Application.IntegrationEvents.Events;

namespace FastRegistrator.Application.Mappings
{
    internal class IntegrationEventsToCommandsProfile : Profile
    {
        public IntegrationEventsToCommandsProfile()
        {
            CreateMap<ESIAApprovedEvent, StartRegistrationCommand>();
        }
    }
}
