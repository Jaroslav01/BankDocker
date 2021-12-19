using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Transactions.EventHandlers;

public class TransactionCreatedEventHandler : INotificationHandler<DomainEventNotification<TransactionCreatedEvent>>
{
    private readonly ILogger<TransactionCreatedEventHandler> _logger;
    private readonly IApplicationDbContext _context;
    private readonly IDateTime _dateTime;

    public TransactionCreatedEventHandler(ILogger<TransactionCreatedEventHandler> logger, IApplicationDbContext context, IDateTime dateTime)
    {
        _logger = logger;
        _context = context;
        _dateTime = dateTime;
    }

    public Task Handle(DomainEventNotification<TransactionCreatedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        var transceiverAccount = _context.Accounts.FirstOrDefault(account => account.AccountNumber == domainEvent.Transaction.TransceiverAccountNumber);
        var receiverAccount = _context.Accounts.FirstOrDefault(account => account.AccountNumber == domainEvent.Transaction.ReceiverAccountNumber);

        if (transceiverAccount == null) throw new NotImplementedException("transceiverAccount == null");
        if (receiverAccount == null) throw new NotImplementedException("receiverAccount == null");

        transceiverAccount.Amount -= domainEvent.Transaction.Amount;
        receiverAccount.Amount += domainEvent.Transaction.Amount;

        transceiverAccount.DomainEvents.Add(new AccountCreatedEvent(transceiverAccount));
        receiverAccount.DomainEvents.Add(new AccountCreatedEvent(receiverAccount));

        _context.SaveChangesAsync(cancellationToken).Wait();

        _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);

        return Task.CompletedTask;
    }
}
