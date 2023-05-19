using BudgetRequests.DomainModels.Admins;
using BudgetRequests.Models.BudgetRequests;

namespace BudgetRequests.DomainModels.BudgetRequests;

public record AdminSignatory(
    Admin Admin,
    AdminPosition AdminPosition,
    bool HasSigned): 
    Signatory(
        Admin,
        AdminPosition switch
        {
            AdminPosition.AssistantDean => "Assistant Dean",
            AdminPosition.Dean => "Dean",
            AdminPosition.StudentAffairsDirector => "Student Affairs Director",
            _ => ""
        },
        HasSigned);