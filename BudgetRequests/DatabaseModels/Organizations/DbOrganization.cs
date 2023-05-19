using System.ComponentModel.DataAnnotations.Schema;
using BudgetRequests.DatabaseModels.Users;
using BudgetRequests.Models;

namespace BudgetRequests.DatabaseModels.Organizations;

public class DbOrganization
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DbUser? Adviser { get; set; }
    public bool IsStudentCouncil { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal GrossBudget { get; set; }
}