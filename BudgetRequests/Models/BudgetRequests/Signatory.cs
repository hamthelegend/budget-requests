using BudgetRequests.Models.Organizations;

namespace BudgetRequests.Models.BudgetRequests;

public class Signatory
{
    public int Id { get; set; }
    public OfficerRole OfficerRole { get; set; }
    public bool HasSigned { get; set; }
}