namespace BudgetRequests.DomainModels.Users;

public record User(
    int Id, 
    UserType Type, 
    string FirstName, 
    string? MiddleName, 
    string LastName, 
    string Username, 
    string PasswordHash, 
    string PasswordSalt);