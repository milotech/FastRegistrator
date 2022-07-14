using MediatR;

namespace FastRegistrator.ApplicationCore.Commands.CheckUserByMobilePhone
{
    public record class CheckUserByMobilePhoneCommand : IRequest<bool>
    {
        public string? MobilePhone { get; init; }
    }

    public class CheckUserByMobilePhoneCommandHandler : IRequestHandler<CheckUserByMobilePhoneCommand, bool>
    {
        public CheckUserByMobilePhoneCommandHandler() 
        {
        }

        public async Task<bool> Handle(CheckUserByMobilePhoneCommand request, CancellationToken cancellationToken) 
        {
            return true;
        }
    }
}
