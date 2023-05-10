﻿using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using BudgetRequests.Data;
using BudgetRequests.Models;
using BudgetRequests.Models.Admins;
using BudgetRequests.Models.Organizations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BudgetRequests.Pages.CreateUser;

public class IndexModel : PageModel
{
    private readonly DatabaseContext _context;

    public IndexModel(DatabaseContext context)
    {
        _context = context;
    }

    public string PageTitle { get; set; }

    public bool HasSuperAdmin { get; set; }

    [BindProperty]
    [Display(Name = "First name")]
    public string FirstName { get; set; }

    [BindProperty]
    [Display(Name = "Middle name")]
    public string? MiddleName { get; set; }

    [BindProperty]
    [Display(Name = "Last name")]
    public string LastName { get; set; }

    [BindProperty]
    [Display(Name = "Username")]
    public string Username { get; set; }

    [BindProperty]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [BindProperty]
    [Display(Name = "Repeat password")]
    public string RepeatPassword { get; set; }

    [BindProperty] public string? UserType { get; set; }

    public IActionResult OnGetAsync()
    {
        HasSuperAdmin = _context.HasSuperAdmin();

        if (HasSuperAdmin)
        {
            var user = HttpContext.Session.GetLoggedInUser(_context);
            if (user == null) return RedirectToPage("../Login/Index");

            if (user is not Admin admin ||
                _context.GetAdminRoles(admin).All(x => x.Position != AdminPosition.SuperAdmin))
                return RedirectToPage("../HomePage/Index"); // TODO: Show an error that they should be a super admin
        }

        PageTitle = !HasSuperAdmin ? "Create super admin" : "Create user";
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var isUsernameDuplicate = _context.GetUsers().Any(x => x.Username == Username);

        if (!ModelState.IsValid || isUsernameDuplicate || Password != RepeatPassword)
        {
            return Page();
        }

        var passwordSalt = Hash.GenerateSalt();
        var passwordHash = Password.ComputeHash(passwordSalt);

        var user = UserType == "admin" || !HasSuperAdmin
            ? new Admin
            {
                FirstName = FirstName,
                MiddleName = MiddleName,
                LastName = LastName,
                Username = Username,
                PasswordHash = passwordHash,
                PasswordSalt = Convert.ToBase64String(passwordSalt)
            }
            : new Officer
            {
                FirstName = FirstName,
                MiddleName = MiddleName,
                LastName = LastName,
                Username = Username,
                PasswordHash = passwordHash,
                PasswordSalt = Convert.ToBase64String(passwordSalt)
            } as User;

        _context.AddUser(user);

        if (!HasSuperAdmin)
        {
            var superAdminRole = new AdminRole
            {
                Admin = (user as Admin)!,
                Position = AdminPosition.SuperAdmin
            };
            _context.AddAdminRole(superAdminRole);
        }

        await _context.SaveChangesAsync();

        var isNotLoggedIn = !HttpContext.Session.IsLoggedIn();
        if (isNotLoggedIn)
        {
            HttpContext.Session.Login(user);
        }
        
        return RedirectToPage("../HomePage/Index");
    }

    public void SetUserType(string userType)
    {
        UserType = userType;
    }
}