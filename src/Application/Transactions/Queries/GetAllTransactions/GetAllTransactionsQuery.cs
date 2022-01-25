using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.Transactions.Queries.GetAllTransactions;

public class GetAllTransactionsQuery : IRequest<List<Transaction>>
{
}

public class GetAllTransactionsQueryHandler : IRequestHandler<GetAllTransactionsQuery, List<Transaction>>
{
    private readonly IApplicationDbContext _context;
    public GetAllTransactionsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<Transaction>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
    {
        return _context.Transactions.ToList();
    }
}