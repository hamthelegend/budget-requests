using BudgetRequests.DomainModels.Users;

namespace BudgetRequests.DomainModels.Organizations;

public record Organization(
    int Id, 
    string Name, 
    User Adviser,
    bool IsStudentCouncil, 
    decimal GrossBudget);
