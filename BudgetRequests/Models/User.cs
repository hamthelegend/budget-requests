namespace BudgetRequests.Models;

public abstract class User
{
    public abstract int Id { get; set; }
    public abstract string FirstName { get; set; }
    public abstract string? MiddleName { get; set; }
    public abstract string LastName { get; set; }
    public abstract string Username { get; set; }
    public abstract string PasswordHash { get; set; }
    public abstract string PasswordSalt { get; set; }
}