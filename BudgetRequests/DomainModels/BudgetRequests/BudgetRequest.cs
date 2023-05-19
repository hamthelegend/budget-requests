using BudgetRequests.DomainModels.Organizations;
using BudgetRequests.Models;

namespace BudgetRequests.DomainModels.BudgetRequests;

public record BudgetRequest(
    string Title,
    string Body,
    Organization Requester,
    List<Signatory> Signatories,
    int Id = DatabaseContext.NoId);