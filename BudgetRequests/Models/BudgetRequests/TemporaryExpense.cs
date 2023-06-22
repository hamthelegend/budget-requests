using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetRequests.Models.BudgetRequests;

public class TemporaryExpense
{
    public int Id { get; set; }
    public string Purpose { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }
    public User Author { get; set; }
    public int CreationId { get; set; }
}