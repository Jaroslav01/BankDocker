using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Domain.Entities;

public class Account : AuditableEntity, IHasDomainEvent
{
    public int Id { get; set; }
    public string ApplicationUserId { get; set; }
    public string? AccountNumber { get; set; }
    public string? Name { get; set; }
    public long Amount { get; set; }
    public IList<Transaction> Transactions { get; private set; } = new List<Transaction>();
    private bool _done;
    public bool Done
    {
        get => _done;
        set
        {
            if (value == true && _done == false)
            {
                DomainEvents.Add(new AccountCompletedEvent(this));
            }

            _done = value;
        }
    }
    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}
