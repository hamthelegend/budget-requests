using BudgetRequests.Models.Organizations;

namespace BudgetRequests.Models.BudgetRequests;

public class BudgetRequest
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public Organization Requester { get; set; }
}