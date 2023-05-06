using System.ComponentModel.DataAnnotations.Schema;
using BudgetRequests.Models.Admins;

namespace BudgetRequests.Models.Organizations;

public class Organization
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Admin Adviser { get; set; }
    public bool IsStudentCouncil { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal GrossBudget { get; set; }
}