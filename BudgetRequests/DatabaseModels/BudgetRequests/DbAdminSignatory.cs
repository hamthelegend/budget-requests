using BudgetRequests.DatabaseModels.Admins;

namespace BudgetRequests.DatabaseModels.BudgetRequests;

public class DbAdminSignatory
{
    public int Id { get; set; }
    public DbBudgetRequest BudgetRequest { get; set; }
    public DbAdminRole? Role { get; set; }
    public bool HasSigned { get; set; }
}