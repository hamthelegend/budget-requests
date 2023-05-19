namespace BudgetRequests.DomainModels.BudgetRequests;

public record Expense(
    string Purpose,
    decimal Amount);