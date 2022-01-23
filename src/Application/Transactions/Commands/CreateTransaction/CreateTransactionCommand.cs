using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Events;
using EventStore.Client;
using MediatR;

namespace CleanArchitecture.Application.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommand : IRequest<int>
{
    public string SenderAccount { get; set; }
    public string ReceiverAccount { get; set; }
    public long Amount { get; set; }
    public string Description { get; set; }
}
public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, int>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IEventStoreDb _eventStoreDb;
    private readonly IApplicationDbContext _context;

    public CreateTransactionCommandHandler(
        ICurrentUserService currentUserService,
        IEventStoreDb eventStoreDb,
        IApplicationDbContext context
    )
    {
        _currentUserService = currentUserService;
        _context = context;
        _eventStoreDb = eventStoreDb;
    }

    public async Task<int> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var senderAccount = _context.Accounts.FirstOrDefault(
            account => account.AccountNumber == request.SenderAccount && account.ApplicationUserId == _currentUserService.UserId
        );
        var receiverAccount = _context.Accounts.FirstOrDefault(
            account => account.AccountNumber == request.ReceiverAccount
        );

        if (senderAccount?.Amount - request.Amount >= 0)
        {
            var entity = new Transaction
            {
                ReceiverAccountNumber = request.ReceiverAccount,
                SenderAccountNumber = request.SenderAccount,
                Amount = request.Amount,
                Description = request.Description
            };
            entity.DomainEvents.Add(new TransactionCreatedEvent(entity));

            _context.Transactions.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await _eventStoreDb.Save("Transaction", "Transactions", entity, cancellationToken);

            return entity.Id;
        }
        return 0;
    }
}