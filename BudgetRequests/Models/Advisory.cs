using BudgetRequests.Models.Organizations;

namespace BudgetRequests.Models;

public class Advisory
{
    public int Id { get; set; }
    public User Adviser { get; set; }
    public Organization Organization { get; set; }
}