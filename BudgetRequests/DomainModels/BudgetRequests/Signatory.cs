using BudgetRequests.DomainModels.Users;

namespace BudgetRequests.DomainModels.BudgetRequests;

public abstract record Signatory(
    User User,
    string Position,
    bool HasSigned);