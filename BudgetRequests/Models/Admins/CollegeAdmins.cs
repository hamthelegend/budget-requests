namespace BudgetRequests.Models.Admins;

public record CollegeAdmins(
    User? AssistantDean,
    User? Dean,
    User? StudentAffairsDirector)
{
    public List<User?> ToList()
    {
        return new List<User?> { AssistantDean, Dean, StudentAffairsDirector };
    }
}