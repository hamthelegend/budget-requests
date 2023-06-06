using BudgetRequests.Models.Users;

namespace BudgetRequests.Models.Admins;

public class AdminRole : UserRole
{
    public int Id { get; set; }
    public User Admin { get; set; }
    public AdminPosition Position { get; set; }
}