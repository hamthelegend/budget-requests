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

    public List<Signatory> SignatoriesWhoCanSign()
    {
        return GetSigningStage() switch
        {
            SigningStage.Organization => new List<Signatory> { Treasurer, Auditor, President, Adviser },
            SigningStage.Deans => new List<Signatory> { Treasurer, Auditor, President, Adviser, AssistantDean, Dean },
            SigningStage.StudentAffairsDirector => ToList()
        };
    }

    public List<Signatory> ToList()
    {
        return new()
        {
            Treasurer, Auditor, President, Adviser, AssistantDean, Dean, StudentAffairsDirector
        };
    }
}