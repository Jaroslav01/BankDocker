using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Domain.Entities;

public class Transaction : AuditableEntity, IHasDomainEvent
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public long Amount { get; set; }
    public string SenderAccountNumber { get; set; }
    public string ReceiverAccountNumber { get; set; }
    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}
