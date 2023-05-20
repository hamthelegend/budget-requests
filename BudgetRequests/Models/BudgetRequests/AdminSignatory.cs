using BudgetRequests.Models.Admins;

namespace BudgetRequests.Models.BudgetRequests;

public class AdminSignatory
{
    public int Id { get; set; }
    public BudgetRequest BudgetRequest { get; set; }
    public AdminRole? Role { get; set; }
    public bool HasSigned { get; set; }
}