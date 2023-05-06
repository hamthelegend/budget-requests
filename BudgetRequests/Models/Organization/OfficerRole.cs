namespace BudgetRequests.Models;

public class OfficerRole
{
    public int Id { get; set; }
    public Officer Officer { get; set; }
    public Organization Organization { get; set; }
    public OrganizationPosition Position { get; set; }
}