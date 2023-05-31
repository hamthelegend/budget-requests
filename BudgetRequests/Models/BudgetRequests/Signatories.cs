namespace BudgetRequests.Models.BudgetRequests;

public record Signatories(
    OfficerSignatory Treasurer,
    OfficerSignatory Auditor,
    OfficerSignatory President,
    AdminSignatory Adviser,
    AdminSignatory AssistantDean,
    AdminSignatory Dean,
    AdminSignatory StudentAffairsDirector)
{
    public SigningStage GetSigningStage()
    {
        if (StudentAffairsDirector.HasSigned) 
            return SigningStage.Approved;
        if (Dean.HasSigned && AssistantDean.HasSigned) 
            return SigningStage.StudentAffairsDirector;
        if (Adviser.HasSigned && President.HasSigned && Auditor.HasSigned && Treasurer.HasSigned) 
            return SigningStage.Deans;
        return SigningStage.Organization;
    }
}