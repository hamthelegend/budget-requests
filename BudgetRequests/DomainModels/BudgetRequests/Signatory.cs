using BudgetRequests.DomainModels.Users;

namespace BudgetRequests.DomainModels.BudgetRequests;

public record Signatory(
    User User,
    string Position,
    bool HasSigned);