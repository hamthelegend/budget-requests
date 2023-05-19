using BudgetRequests.Models.BudgetRequests;
using BudgetRequests.Models.Organizations;

namespace BudgetRequests.Models.Admins;

public class AdminSignatory
{
    public int Id { get; set; }
    public BudgetRequest BudgetRequest { get; set; }
    public AdminRole Role { get; set; }
    public bool HasSigned { get; set; }
}