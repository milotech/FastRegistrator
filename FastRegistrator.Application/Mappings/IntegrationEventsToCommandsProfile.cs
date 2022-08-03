using AutoMapper;
using FastRegistrator.ApplicationCore.Commands.SetStatusESIAApproved;
using FastRegistrator.ApplicationCore.IntegrationEvents.Events;

namespace FastRegistrator.ApplicationCore.Mappings
{
    internal class IntegrationEventsToCommandsProfile : Profile
    {
        public IntegrationEventsToCommandsProfile()
        {
            CreateMap<ESIAApprovedEvent, SetStatusESIAApprovedCommand>();
        }
    }
}
