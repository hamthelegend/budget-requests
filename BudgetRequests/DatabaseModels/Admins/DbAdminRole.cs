using BudgetRequests.DatabaseModels.Users;
using BudgetRequests.DomainModels;
using BudgetRequests.Models;

namespace BudgetRequests.DatabaseModels.Admins;

public class DbAdminRole
{
    public int Id { get; set; }
    public DbUser Admin { get; set; }
    public AdminPosition Position { get; set; }
}