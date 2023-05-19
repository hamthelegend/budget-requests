using BudgetRequests.DomainModels.Admins;
using BudgetRequests.Models;

namespace BudgetRequests.DomainModels.Users;

public abstract record User(
    string FirstName,
    string? MiddleName,
    string LastName,
    string Username,
    string PasswordHash,
    string PasswordSalt,
    int Id = DatabaseContext.NoId)
{
    public UserType Type => this is Admin ? UserType.Admin : UserType.Officer;
};