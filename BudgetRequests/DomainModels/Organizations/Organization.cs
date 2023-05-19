using BudgetRequests.DomainModels.Users;
using BudgetRequests.Models;

namespace BudgetRequests.DomainModels.Organizations;

public record Organization(
    string Name,
    User Adviser,
    bool IsStudentCouncil,
    decimal GrossBudget,
    int Id = DatabaseContext.NoId);