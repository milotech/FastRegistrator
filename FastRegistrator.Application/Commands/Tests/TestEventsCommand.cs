using FastRegistrator.ApplicationCore.Events;
using FastRegistrator.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastRegistrator.ApplicationCore.Commands.Tests
{
    public record TestEventsCommand(string Name, int Age) : IRequest;

    public class TestEventsCommandHandler: AsyncRequestHandler<TestEventsCommand>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly IPublisher _publisher;

        public TestEventsCommandHandler(IApplicationDbContext dbContext, ILogger<TestEventsCommandHandler> logger, IPublisher publisher)
        {
            _dbContext = dbContext;
            _logger = logger;
            _publisher = publisher;

            DbContextCache.Context1 = _dbContext;
        }

        protected override Task Handle(TestEventsCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("TestEventsCommand Started.");

            _publisher.Publish(new PersonCheckedTestEvent(request.Name, request.Age), cancellationToken);

            _logger.LogInformation("TestEventsCommand Completed.");

            return Task.CompletedTask;
        }
    }
}
