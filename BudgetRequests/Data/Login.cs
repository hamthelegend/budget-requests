using System.Security.Claims;
using BudgetRequests.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BudgetRequests.Data;

public static class Login
{
    public static AuthenticationProperties AuthProperties = new AuthenticationProperties
    {
        AllowRefresh = true,
        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(1440),
        IsPersistent = true,
        IssuedUtc = DateTimeOffset.UtcNow
    };

    public static ClaimsIdentity GetClaimIdentity(User user)
    {
        var claims = new List<Claim>()
        {
            new("UserId", user.Id.ToString())
        };

        return new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);
    }
}