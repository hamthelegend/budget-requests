using BudgetRequests.DomainModels.Users;
using BudgetRequests.Models;

namespace BudgetRequests.DomainModels.Organizations;

public record Officer(
    string FirstName,
    string? MiddleName,
    string LastName,
    string Username,
    string PasswordHash,
    string PasswordSalt,
    List<OfficerRole> Roles,
    int Id = DatabaseContext.NoId) :
    User(Id: Id, FirstName: FirstName, MiddleName: MiddleName, LastName: LastName,
        Username: Username, PasswordHash: PasswordHash, PasswordSalt: PasswordSalt);