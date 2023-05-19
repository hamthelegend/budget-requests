using BudgetRequests.DomainModels.Users;
using BudgetRequests.Models.Admins;

namespace BudgetRequests.DomainModels.Admins;

public record Admin(
    int Id,
    string FirstName,
    string? MiddleName,
    string LastName,
    string Username,
    string PasswordHash,
    string PasswordSalt,
    List<AdminRole> Roles) :
    User(Id, UserType.Admin, FirstName, MiddleName, LastName, Username, PasswordHash, PasswordSalt);