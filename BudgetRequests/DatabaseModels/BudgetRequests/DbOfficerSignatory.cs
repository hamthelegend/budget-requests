using BudgetRequests.DatabaseModels.Organizations;

namespace BudgetRequests.DatabaseModels.BudgetRequests;

public class DbOfficerSignatory
{
    public int Id { get; set; }
    public DbBudgetRequest BudgetRequest { get; set; }
    public DbOfficerRole? Role { get; set; }
    public bool HasSigned { get; set; }
}