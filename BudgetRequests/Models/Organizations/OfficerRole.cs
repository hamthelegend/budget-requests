using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using BudgetRequests.Models.Users;

namespace BudgetRequests.Models.Organizations;

public class OfficerRole : UserRole
{
    public int Id { get; set; }
    public User Officer { get; set; }
    public Organization Organization { get; set; }
    public OrganizationPosition Position { get; set; }
}