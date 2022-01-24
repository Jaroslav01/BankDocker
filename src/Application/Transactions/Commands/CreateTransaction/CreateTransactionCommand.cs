using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Transactions.Queries.GetTransactionsByUserIdQuery;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Events;
using EventStore.Client;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Application.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommand : IRequest<int>
{
    public string SenderAccount { get; set; }
    public string ReceiverAccount { get; set; }
    public long Amount { get; set; }
    public string Description { get; set; }
    public int CommissionType { get; set; }
}
public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, int>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IEventStoreDb _eventStoreDb;
    private readonly IApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ISender _mediator;

    public CreateTransactionCommandHandler(
        ICurrentUserService currentUserService,
        IEventStoreDb eventStoreDb,
        IApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        ISender mediator
    )
    {
        _currentUserService = currentUserService;
        _context = context;
        _eventStoreDb = eventStoreDb;
        _userManager = userManager;
        _mediator = mediator;
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

            senderAccount.Amount -= request.Amount;
            receiverAccount.Amount += request.Amount;

            var bankUser = _userManager.FindByNameAsync("bankUser@localhost").Result;
            var query = new GetTransactionsByUserIdQuery()
            {
                UserId = bankUser.Id
            };
            var bankAccounts = _mediator.Send(query).Result;
            var bankAccount = bankAccounts.FirstOrDefault(account => account.Name == "Bank account");

            long commission = 250;

            if (senderAccount.ApplicationUserId == receiverAccount.ApplicationUserId)
            {
                commission = 0;
            }
            else if (request.Amount > 10000)
            {
                commission = (long)Math.Round(request.Amount * 0.025);
            }
            
            if (request.CommissionType == CommissionType.FromSender)
            {
                senderAccount.Amount -= commission;
                bankAccount.Amount += commission;
                entity.CommissionFromTheSender = commission;
                entity.CommissionFromTheReceiver = 0;
            }
            else if (request.CommissionType == CommissionType.FromReceiver)
            {
                receiverAccount.Amount -= commission;
                bankAccount.Amount += commission;
                entity.CommissionFromTheReceiver = commission;
                entity.CommissionFromTheSender = 0;
            }
            else if (request.CommissionType == CommissionType.InHalf)
            {
                senderAccount.Amount -= commission / 2;
                receiverAccount.Amount -= commission / 2;
                bankAccount.Amount += commission;
                entity.CommissionFromTheSender = commission / 2;
                entity.CommissionFromTheReceiver = commission / 2;
            }

            senderAccount.DomainEvents.Add(new AccountCreatedEvent(senderAccount));
            receiverAccount.DomainEvents.Add(new AccountCreatedEvent(receiverAccount));

            await _context.SaveChangesAsync(cancellationToken);
            await _eventStoreDb.Save("Transaction", "Transactions", entity, cancellationToken);

            return entity.Id;
        }
        return 0;
    }
}