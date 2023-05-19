namespace BudgetRequests.DomainModels.Organizations;

public record OfficerRole(
    Organization Organization,
    OrganizationPosition Position);