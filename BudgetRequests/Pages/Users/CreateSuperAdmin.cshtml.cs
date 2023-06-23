using System.ComponentModel.DataAnnotations;
using BudgetRequests.Data;
using BudgetRequests.Models;
using BudgetRequests.Models.Admins;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace BudgetRequests.Pages.Users;

public class CreateSuperAdminModel : PageModel
{
    private readonly DatabaseContext _context;

    public CreateSuperAdminModel(DatabaseContext context)
    {
        _context = context;
    }

    [BindProperty] public string? FirstName { get; set; }
    public string? FirstNameError { get; set; }

    [BindProperty] public string? MiddleName { get; set; }
    public string? MiddleNameError { get; set; }

    [BindProperty] public string? LastName { get; set; }
    public string? LastNameError { get; set; }

    [BindProperty] public string? Username { get; set; }
    public string? UsernameError { get; set; }

    [BindProperty] public string? Password { get; set; }
    public string? PasswordError { get; set; }

    [BindProperty] public string? RepeatPassword { get; set; }
    public string? RepeatPasswordError { get; set; }

    public IActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPostAdd()
    {
        var hasError = false;

        if (FirstName.IsNullOrEmpty())
        {
            FirstNameError = "First name is required";
            hasError = true;
        }

        if (LastName.IsNullOrEmpty())
        {
            LastNameError = "Last name is required";
            hasError = true;
        }

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

        if (RepeatPassword.IsNullOrEmpty())
        {
            RepeatPasswordError = "Repeat password is required";
            hasError = true;
        }

        if (hasError)
        {
            return Page();
        }

        if (!_context.IsUsernameAvailable(Username!))
        {
            UsernameError = "That username is already taken";
            hasError = true;
        }

        if (Password != RepeatPassword)
        {
            RepeatPasswordError = "Passwords do not match";
            hasError = true;
        }

        if (hasError)
        {
            return Page();
        }

        var passwordSalt = Hash.GenerateSalt();
        var passwordHash = Password!.ComputeHash(passwordSalt);

        var user = new User
        {
            Type = Models.UserType.Admin,
            FirstName = FirstName!,
            MiddleName = MiddleName,
            LastName = LastName!,
            Username = Username!,
            PasswordHash = passwordHash,
            PasswordSalt = Convert.ToBase64String(passwordSalt)
        };

        _context.AddUser(user);

        var superAdminRole = new AdminRole
        {
            Admin = user,
            Position = AdminPosition.SuperAdmin
        };
        _context.AddAdminRole(superAdminRole);

        _context.SaveChanges();

        var isNotLoggedIn = !HttpContext.Session.IsLoggedIn();
        if (isNotLoggedIn)
        {
            HttpContext.Session.Login(user);
        }

        return RedirectToPage("../Requests/Index");
    }

    public IActionResult OnPostCancel(int id)
    {
        OnGet();
        return RedirectToPage("./Index");
    }
}