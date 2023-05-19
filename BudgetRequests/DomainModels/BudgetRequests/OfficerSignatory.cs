using BudgetRequests.DomainModels.Organizations;

namespace BudgetRequests.DomainModels.BudgetRequests;

public record OfficerSignatory(
    Officer Officer,
    OfficerRole OfficerRole,
    bool HasSigned) :
    Signatory(
        Officer,
        $"{OfficerRole.Position switch {
            OrganizationPosition.President => "President",
            OrganizationPosition.VicePresident => "Vice President",
            OrganizationPosition.Secretary => "Secretary",
            OrganizationPosition.Treasurer => "Treasurer",
            OrganizationPosition.Auditor => "Auditor",
            OrganizationPosition.PublicRelationsOfficer => "P.R.O.",
            _ => "" }
        }, {OfficerRole.Organization.Name}",
        HasSigned);