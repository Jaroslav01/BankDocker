namespace CleanArchitecture.Domain.Events;

public class AccountCompletedEvent : DomainEvent
{
    public AccountCompletedEvent(Account account)
    {
        Account = account;
    }

    public Account Account { get; }
}
