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

public class CreatePersonCommandHandler : IRequestHandler<CreateAccountCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreatePersonCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        if (request.Name == null)
            request.Name = "My Account";
        static ulong RandomDigits(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return ulong.Parse(s);
        }
        var accountNumber = RandomDigits(8);
        var time = DateTime.Now;
        var entity = new Account
        {
            ApplicationUserId = request.ApplicationUserId,
            AccountNumber = accountNumber,
            Name = request.Name,
            Amount = 0,
            CreatedDate = time,
            LastModified = time,
        };

        entity.DomainEvents.Add(new AccountCreatedEvent(entity));

        _context.Accounts.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
