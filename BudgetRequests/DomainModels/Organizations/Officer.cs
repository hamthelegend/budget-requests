using BudgetRequests.DomainModels.Users;
using BudgetRequests.Models.Organizations;

namespace BudgetRequests.DomainModels.Organizations;

public record Officer(
    int Id,
    Models.Organizations.OfficerRole Role,
    string FirstName,
    string? MiddleName,
    string LastName,
    string Username,
    string PasswordHash,
    string PasswordSalt) :
    User(Id, UserType.Officer, FirstName, MiddleName, LastName, Username, PasswordHash, PasswordSalt);