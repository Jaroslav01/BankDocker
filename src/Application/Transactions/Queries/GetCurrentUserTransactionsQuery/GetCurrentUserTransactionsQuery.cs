using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Accounts.Queries.GetCurrentUserAccounts;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.Transactions.Queries.GetCurrentUserTransactionsQuery;

public class GetCurrentUserTransactionsQuery : IRequest<List<Transaction>>
{
}

public class GetCurrentUserTransactionsQueryHandler : IRequestHandler<GetCurrentUserTransactionsQuery, List<Transaction>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly ISender _mediator;
    public GetCurrentUserTransactionsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, ISender mediator)
    {
        _context = context;
        _currentUserService = currentUserService;
        _mediator = mediator;
    }

    private bool IsContainedAccountNumber(List<Account> accounts, Transaction transaction)
    {
        return accounts.FirstOrDefault(account =>
            account.AccountNumber == transaction.ReceiverAccountNumber ||
            account.AccountNumber == transaction.SenderAccountNumber) != null;
    }
    public async Task<List<Transaction>> Handle(GetCurrentUserTransactionsQuery request, CancellationToken cancellationToken)
    {
        var accounts = await _mediator.Send(new GetCurrentUserAccountsQuery());
        var transactions = _context.Transactions.Where(transaction => IsContainedAccountNumber(accounts, transaction)).ToList();//ERROR
        return transactions;
    }
}