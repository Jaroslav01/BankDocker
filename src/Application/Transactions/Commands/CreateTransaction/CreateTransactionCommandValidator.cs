using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CleanArchitecture.Application.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(v => v.ReceiverAccount)
            .MaximumLength(200)
            .NotEmpty();
        RuleFor(v => v.SenderAccount)
            .MaximumLength(200)
            .NotEmpty();

    }
}
