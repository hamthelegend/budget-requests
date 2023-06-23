namespace BudgetRequests.Models.Organizations;

public record OrganizationOfficers(
    OfficerRole President,
    OfficerRole VicePresident,
    OfficerRole Secretary,
    OfficerRole Treasurer,
    OfficerRole Auditor,
    OfficerRole PublicRelationsOfficer)
{

    public OrganizationOfficers(
        Organization organization,
        User president,
        User vicePresident,
        User secretary,
        User treasurer,
        User auditor,
        User publicRelationsOfficer) : this(
        President: new OfficerRole
        {
            Organization = organization,
            Officer = president,
            Position = OrganizationPosition.President,
        },
        VicePresident: new OfficerRole
        {
            Organization = organization,
            Officer = vicePresident,
            Position = OrganizationPosition.VicePresident,
        },
        Secretary: new OfficerRole
        {
            Organization = organization,
            Officer = secretary,
            Position = OrganizationPosition.Secretary,
        },
        Treasurer: new OfficerRole
        {
            Organization = organization,
            Officer = treasurer,
            Position = OrganizationPosition.Treasurer,
        },
        Auditor: new OfficerRole
        {
            Organization = organization,
            Officer = auditor,
            Position = OrganizationPosition.Auditor,
        },
        PublicRelationsOfficer: new OfficerRole
        {
            Organization = organization,
            Officer = publicRelationsOfficer,
            Position = OrganizationPosition.PublicRelationsOfficer,
        })
    {
        
    }
    public List<OfficerRole> ToList()
    {
        return new()
        {
            President,
            VicePresident,
            Secretary,
            Treasurer,
            Auditor,
            PublicRelationsOfficer
        };
    }
}