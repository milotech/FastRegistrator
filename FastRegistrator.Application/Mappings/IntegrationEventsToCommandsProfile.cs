using AutoMapper;
using FastRegistrator.ApplicationCore.Commands.CheckPersonByPhone;
using FastRegistrator.ApplicationCore.Commands.SetStatusESIANotApproved;
using FastRegistrator.ApplicationCore.IntegrationEvents.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastRegistrator.ApplicationCore.Mappings
{
    internal class IntegrationEventsToCommandsProfile : Profile
    {
        public IntegrationEventsToCommandsProfile()
        {
            CreateMap<PersonCheckRequestedEvent, CheckPersonByPhoneCommand>();
            CreateMap<ESIANotApprovedEvent, SetStatusESIANotApprovedCommand>()
                .ForMember(dest => dest.RejectReason, opt => opt.MapFrom(src => src.Message));
        }
    }
}
