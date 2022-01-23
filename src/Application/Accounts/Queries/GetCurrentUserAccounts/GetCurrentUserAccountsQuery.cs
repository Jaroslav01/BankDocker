using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.Accounts.Queries.GetCurrentUserAccounts;

public class GetCurrentUserAccountsQuery : IRequest<List<Account>>
{
}

public class GetCurrentUserAccountsQueryHandler : IRequestHandler<GetCurrentUserAccountsQuery, List<Account>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetCurrentUserAccountsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<List<Account>> Handle(GetCurrentUserAccountsQuery request, CancellationToken cancellationToken)
    {
        var accounts = _context.Accounts.Where(account => account.ApplicationUserId == _currentUserService.UserId).ToList();
        return accounts;
    }
}