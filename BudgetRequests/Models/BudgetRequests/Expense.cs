using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetRequests.Models.BudgetRequests;

public class Expense
{
    public int Id { get; set; }
    public BudgetRequest BudgetRequest { get; set; }
    public string Purpose { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }
}