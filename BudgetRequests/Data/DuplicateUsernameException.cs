namespace BudgetRequests.Data;

public class DuplicateUsernameException : Exception
{
    public string Username { get; }
    
    public DuplicateUsernameException(string username): base($"The username {username} already exists.")
    {
        Username = username;
    }
}