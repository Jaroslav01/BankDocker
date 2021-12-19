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
    public string? Description { get; set; }
    public long Amount { get; set; }
    public string TransceiverAccountNumber { get; set; }
    public string ReceiverAccountNumber { get; set; }
}
public class CreateeTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IEventStoreDb _eventStoreDb;

    public CreateeTransactionCommandHandler(IApplicationDbContext context, IEventStoreDb eventStoreDb)
    {
        _context = context;
        _eventStoreDb = eventStoreDb;
    }

    public async Task<int> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var entity = new Transaction
        {
            Description = request.Description,
            Amount = request.Amount,
            ReceiverAccountNumber = request.ReceiverAccountNumber,
            TransceiverAccountNumber = request.TransceiverAccountNumber
        };
        await _eventStoreDb.Save("Transaction", "Transactions", entity, cancellationToken);
        entity.DomainEvents.Add(new TransactionCreatedEvent(entity));
        _context.Transactions.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        

        return entity.Id;
    }
}