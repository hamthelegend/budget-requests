using BudgetRequests.Models.Organizations;

namespace BudgetRequests.Models.BudgetRequests;

public class OfficerSignatory : Signatory
{
    public override int Id { get; set; }
    public BudgetRequest BudgetRequest { get; set; }
    public OfficerRole? Role { get; set; }
    public override bool HasSigned { get; set; }

    public override User User => Role.Officer;
}