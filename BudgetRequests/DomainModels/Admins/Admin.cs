using BudgetRequests.DatabaseModels.Admins;
using BudgetRequests.DomainModels.Users;
using BudgetRequests.Models;

namespace BudgetRequests.DomainModels.Admins;

public record Admin(
    string FirstName,
    string? MiddleName,
    string LastName,
    string Username,
    string PasswordHash,
    string PasswordSalt,
    List<AdminPosition> Positions,
    int Id = DatabaseContext.NoId) :
    User(Id: Id, FirstName: FirstName, MiddleName: MiddleName, LastName: LastName,
        Username: Username, PasswordHash: PasswordHash, PasswordSalt: PasswordSalt);