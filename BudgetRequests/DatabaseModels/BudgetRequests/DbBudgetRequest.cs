using BudgetRequests.DatabaseModels.Organizations;

namespace BudgetRequests.DatabaseModels.BudgetRequests;

public class DbBudgetRequest
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public DbOrganization Requester { get; set; }
}