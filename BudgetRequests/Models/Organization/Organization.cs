namespace BudgetRequests.Models;

public class Organization
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Admin.Admin Adviser { get; set; }
    public bool IsStudentCouncil { get; set; }
    public decimal GrossBudget { get; set; }
}