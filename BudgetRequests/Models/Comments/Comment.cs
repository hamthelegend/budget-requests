using BudgetRequests.Models.BudgetRequests;

namespace BudgetRequests.Models.Comments;

public class Comment
{
    public int Id { get; set; }
    public BudgetRequest BudgetRequest { get; set; }
    public User Commenter { get; set; }
    public string Body { get; set; }
}