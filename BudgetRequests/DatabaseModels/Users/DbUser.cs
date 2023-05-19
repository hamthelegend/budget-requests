using BudgetRequests.DomainModels.Users;

namespace BudgetRequests.DatabaseModels.Users;

public class DbUser
{
    public int Id { get; set; }
    public UserType Type { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
}