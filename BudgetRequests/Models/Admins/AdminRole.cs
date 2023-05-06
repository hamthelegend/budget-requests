namespace BudgetRequests.Models.Admins;

public class AdminRole
{
    public int Id { get; set; }
    public Admin Admin { get; set; }
    public AdminPosition Position { get; set; }
}