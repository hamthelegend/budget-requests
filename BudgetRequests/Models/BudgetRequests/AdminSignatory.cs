using BudgetRequests.Models.Admins;

namespace BudgetRequests.Models.BudgetRequests;

public class AdminSignatory : Signatory
{
    public override int Id { get; set; }
    public BudgetRequest BudgetRequest { get; set; }
    public AdminRole? Role { get; set; }
    public override bool HasSigned { get; set; }

    public override User User => Role.Admin;
}