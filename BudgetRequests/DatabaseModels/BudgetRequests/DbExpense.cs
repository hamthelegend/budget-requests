using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetRequests.DatabaseModels.BudgetRequests;

public class DbExpense
{
    public int Id { get; set; }
    public DbBudgetRequest BudgetRequest { get; set; }
    public string Purpose { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }
}