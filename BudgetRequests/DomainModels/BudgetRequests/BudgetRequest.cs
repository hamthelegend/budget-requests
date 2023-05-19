using BudgetRequests.DomainModels.Organizations;

namespace BudgetRequests.DomainModels.BudgetRequests;

public record BudgetRequest(
    int Id,
    string Title,
    string Body,
    Organization Requester,
    List<Signatory> Signatories);