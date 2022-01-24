using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CleanArchitecture.Application.Transactions.Queries.GetTransactionsByUserId;

public class GetTransactionsByUserIdValidator : AbstractValidator<GetTransactionsByUserIdQuery.GetTransactionsByUserIdQuery>
{
    public GetTransactionsByUserIdValidator()
    {
        RuleFor(v => v.UserId)
            .MaximumLength(36)
            .MinimumLength(36)
            .NotEmpty();

    }
}
