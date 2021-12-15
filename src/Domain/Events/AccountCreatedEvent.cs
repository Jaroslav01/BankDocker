namespace CleanArchitecture.Domain.Events;

public class AccountCreatedEvent : DomainEvent
{
    public AccountCreatedEvent(Account account)
    {
        Account = account;
    }

    public Account Account { get; }
}
