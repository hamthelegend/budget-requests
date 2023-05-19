using BudgetRequests.DatabaseModels.Users;
using BudgetRequests.DomainModels.Organizations;
using BudgetRequests.Models;

namespace BudgetRequests.DatabaseModels.Organizations;

public class DbOfficerRole
{
    public int Id { get; set; }
    public DbUser Officer { get; set; }
    public DbOrganization Organization { get; set; }
    public OrganizationPosition Position { get; set; }
}