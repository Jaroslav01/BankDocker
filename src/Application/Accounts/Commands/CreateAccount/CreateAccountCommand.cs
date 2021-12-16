using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Events;
using MediatR;

namespace CleanArchitecture.Application.Accounts.Commands.CreatePerson;

public class CreateAccountCommand : IRequest<int>
{
    public int ApplicationUserId { get; set; }
    public string? Name { get; set; }
}

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTime _dateTime;

    public CreateAccountCommandHandler(IApplicationDbContext context, IDateTime dateTime)
    {
        _context = context;
        _dateTime = dateTime;
    }

    public async Task<int> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        static string RandomDigits(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }
        var accountNumber = RandomDigits(19);
        var entity = new Account
        {
            ApplicationUserId = request.ApplicationUserId,
            AccountNumber = accountNumber,
            Name = request.Name,
            Amount = 0,
            Created = _dateTime.Now,
            LastModified = _dateTime.Now,
        };

        entity.DomainEvents.Add(new AccountCreatedEvent(entity));

        _context.Accounts.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
