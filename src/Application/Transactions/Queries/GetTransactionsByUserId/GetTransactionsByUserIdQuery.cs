using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Accounts.Queries.GetCurrentUserAccounts;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.Transactions.Queries.GetTransactionsByUserIdQuery;

public class GetTransactionsByUserIdQuery : IRequest<List<Account>>
{
    public string UserId { get; set; }
}

public class GetTransactionsByUserIdQueryHandler : IRequestHandler<GetTransactionsByUserIdQuery, List<Account>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly ISender _mediator;
    public GetTransactionsByUserIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, ISender mediator)
    {
        _context = context;
        _currentUserService = currentUserService;
        _mediator = mediator;
    }

    public async Task<List<Account>> Handle(GetTransactionsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var accounts = _context.Accounts.Where(account => account.ApplicationUserId == request.UserId).ToList();
        return accounts;
    }
}