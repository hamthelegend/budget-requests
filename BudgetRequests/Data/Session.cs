using BudgetRequests.Models;

namespace BudgetRequests.Data;

public static class Session
{
    private const string UsernameKey = "_Username";

    public static void Login(this ISession session, User user)
    {
        session.SetString(UsernameKey, user.Username);
    }

    public static void Logout(this ISession session)
    {
        session.SetString(UsernameKey, null!);
    }

    public static User? GetLoggedInUser(this ISession session, DatabaseContext databaseContext)
    {
        var username = session.GetString(UsernameKey);
        return databaseContext.GetUsers().FirstOrDefault(x => x.Username == username);
    }

    public static bool IsLoggedIn(this ISession session)
    {
        var username = session.GetString(UsernameKey);
        return username != null;
    }
}