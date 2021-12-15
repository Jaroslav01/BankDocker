using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Domain.Entities;

public class Transaction : AuditableEntity
{
    [Key]
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string? Description { get; set; }
    public string? Amount { get; set; }
    public DateTime CreatedDate { get; set; }
}
