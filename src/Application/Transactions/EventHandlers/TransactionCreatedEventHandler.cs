using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Accounts.Queries.GetCurrentUserAccounts;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Transactions.Queries.GetTransactionsByUserIdQuery;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Transactions.EventHandlers;

public class TransactionCreatedEventHandler : INotificationHandler<DomainEventNotification<TransactionCreatedEvent>>
{
    private readonly ILogger<TransactionCreatedEventHandler> _logger;
    private readonly IApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ISender _mediator;

    public TransactionCreatedEventHandler(ILogger<TransactionCreatedEventHandler> logger, IApplicationDbContext context, UserManager<ApplicationUser> userManager, ISender mediator)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _mediator = mediator;
    }

    public Task Handle(DomainEventNotification<TransactionCreatedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        var transceiverAccount = _context.Accounts.FirstOrDefault(account => account.AccountNumber == domainEvent.Transaction.SenderAccountNumber);
        var receiverAccount = _context.Accounts.FirstOrDefault(account => account.AccountNumber == domainEvent.Transaction.ReceiverAccountNumber);

        if (transceiverAccount == null) throw new NotImplementedException("transceiverAccount == null");
        if (receiverAccount == null) throw new NotImplementedException("receiverAccount == null");

        

        _context.SaveChangesAsync(cancellationToken).Wait();

        _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);

        return Task.CompletedTask;
    }
}
