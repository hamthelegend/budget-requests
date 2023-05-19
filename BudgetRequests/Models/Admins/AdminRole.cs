using BudgetRequests.DomainModels;

namespace BudgetRequests.Models.Admins;

public class AdminRole
{
    public int Id { get; set; }
    public User Admin { get; set; }
    public AdminPosition Position { get; set; }
}