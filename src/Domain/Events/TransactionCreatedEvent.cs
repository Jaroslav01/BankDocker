namespace CleanArchitecture.Domain.Events;

public class TransactionCreatedEvent : DomainEvent
{
    public TransactionCreatedEvent(Transaction transaction)
    {
        Transaction = transaction;
    }

    public Transaction Transaction { get; }
}
