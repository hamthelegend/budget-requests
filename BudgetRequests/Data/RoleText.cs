using BudgetRequests.Models.Admins;
using BudgetRequests.Models.Organizations;

namespace BudgetRequests.Data;

public static class PositionText
{
    public static string GetText(this AdminPosition adminPosition)
    {
        return adminPosition switch
        {
            AdminPosition.Adviser => "Adviser",
            AdminPosition.AssistantDean => "Assistant Dean",
            AdminPosition.Dean => "Dean",
            AdminPosition.StudentAffairsDirector => "Student Affairs Director",
            AdminPosition.SuperAdmin => "Super Admin",
            _ => throw new ArgumentOutOfRangeException(nameof(adminPosition), adminPosition, null)
        };
    }

    public static string GetText(this OrganizationPosition organizationPosition)
    {
        return organizationPosition switch
        {
            OrganizationPosition.President => "President",
            OrganizationPosition.VicePresident => "Vice President",
            OrganizationPosition.Secretary => "Secretary",
            OrganizationPosition.Treasurer => "Treasurer",
            OrganizationPosition.Auditor => "Auditor",
            OrganizationPosition.PublicRelationsOfficer => "P.R.O.",
            _ => throw new ArgumentOutOfRangeException(nameof(organizationPosition), organizationPosition, null)
        };
    }
}