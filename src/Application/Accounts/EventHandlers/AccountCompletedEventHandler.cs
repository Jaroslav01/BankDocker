using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Accounts.EventHandlers;

public class AccountCompletedEventHandler : INotificationHandler<DomainEventNotification<AccountCompletedEvent>>
{
    private readonly ILogger<AccountCompletedEventHandler> _logger;

    public AccountCompletedEventHandler(ILogger<AccountCompletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<AccountCompletedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);

        return Task.CompletedTask;
    }
}
