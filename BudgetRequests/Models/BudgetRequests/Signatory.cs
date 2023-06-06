namespace BudgetRequests.Models.BudgetRequests;

public abstract class Signatory
{
    public abstract int Id { get; set; }
    public abstract User User { get; }
    public abstract bool HasSigned { get; set; }
}