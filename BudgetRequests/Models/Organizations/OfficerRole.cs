using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetRequests.Models.Organizations;

public class OfficerRole
{
    public int Id { get; set; }
    public User Officer { get; set; }
    public Organization Organization { get; set; }
    public OrganizationPosition Position { get; set; }
}