using BudgetRequests.Models.Organizations;

namespace BudgetRequests.Models.BudgetRequests;

public class OfficerSignatory
{
    public int Id { get; set; }
    public BudgetRequest BudgetRequest { get; set; }
    public OfficerRole? Role { get; set; }
    public bool HasSigned { get; set; }
}