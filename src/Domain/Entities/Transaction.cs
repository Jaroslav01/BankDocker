using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Domain.Entities;

public class Transaction : AuditableEntity, IHasDomainEvent
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public long Amount { get; set; }
    public long CommissionFromTheSender { get; set; }
    public long CommissionFromTheReceiver { get; set; }
    public int CommissionType { get; set; }
    public string SenderAccountNumber { get; set; }
    public string ReceiverAccountNumber { get; set; }
    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}

public class CommissionType
{
    public static readonly int FromSender = 0;
    public static readonly int FromReceiver = 1;
    public static readonly int InHalf = 2;
}