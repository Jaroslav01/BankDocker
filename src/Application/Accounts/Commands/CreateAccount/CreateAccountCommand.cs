using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Events;
using MediatR;

namespace CleanArchitecture.Application.Accounts.Commands.CreateAccount;

public class CreateAccountCommand : IRequest<int>
{
    public string? Name { get; set; }
}

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateAccountCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
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
            ApplicationUserId = _currentUserService.UserId,
            AccountNumber = accountNumber,
            Name = request.Name,
            Amount = 0,
        };

        entity.DomainEvents.Add(new AccountCreatedEvent(entity));

        _context.Accounts.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
