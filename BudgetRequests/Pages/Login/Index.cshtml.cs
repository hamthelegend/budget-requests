using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using BudgetRequests.Data;
using BudgetRequests.Models;
using BudgetRequests.Models.Admins;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace BudgetRequests.Pages.Login;

public class IndexModel : PageModel
{
    private readonly DatabaseContext _context;

    public IndexModel(DatabaseContext context)
    {
        _context = context;
    }
    
    public string? GlobalError { get; set; }

    [BindProperty]
    [Display(Name = "Username")]
    public string? Username { get; set; }

    public string? UsernameError { get; set; }

    [BindProperty]
    [Display(Name = "Password")]
    public string? Password { get; set; }

    public string? PasswordError { get; set; }

    public void OnGet()
    {
        HttpContext.Session.Logout();
    }

    public IActionResult OnPost()
    {
        var hasError = false;

        if (Username.IsNullOrEmpty())
        {
            UsernameError = "Username is required";
            hasError = true;
        }

        if (Password.IsNullOrEmpty())
        {
            PasswordError = "Password is required";
            hasError = true;
        }

        if (hasError)
        {
            return Page();
        }

        var user = _context.GetUser(Username ?? "");
        var passwordHash = Password?.ComputeHash(Convert.FromBase64String(user?.PasswordSalt ?? ""));

        if (user == null || user.PasswordHash != passwordHash)
        {
            GlobalError = "Username or password is incorrect";
            hasError = true;
        }

        if (hasError)
        {
            return Page();
        }

        HttpContext.Session.Login(user!);

        return RedirectToPage("../Requests/Index");
    }
}