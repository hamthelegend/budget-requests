﻿using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using BudgetRequests.Data;
using BudgetRequests.Models;
using BudgetRequests.Models.Admins;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BudgetRequests.Pages.CreateSuperAdmin;

public class IndexModel : PageModel
{
    private readonly DatabaseContext _context;

    public IndexModel(DatabaseContext context)
    {
        _context = context;
    }

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

    public async Task<IActionResult> OnPostAsync()
    {
        var isUsernameDuplicate = _context.GetUsers().Any(x => x.Username == Username);

        if (!ModelState.IsValid || isUsernameDuplicate)
        {
            return Page();
        }

        var passwordSalt = Hash.GenerateSalt();
        var passwordHash = Password.ComputeHash(passwordSalt);

        var superAdmin = new Admin
        {
            FirstName = FirstName,
            MiddleName = MiddleName,
            LastName = LastName,
            Username = Username,
            PasswordHash = passwordHash,
            PasswordSalt = Convert.ToBase64String(passwordSalt)
        };

        _context.AddAdmin(superAdmin);

        var superAdminRole = new AdminRole
        {
            Admin = superAdmin,
            Position = AdminPosition.SuperAdmin
        };

        _context.AddAdminRole(superAdminRole);

        await _context.SaveChangesAsync();

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(Data.Login.GetClaimIdentity(superAdmin)),
            Data.Login.AuthProperties);

        HttpContext.Session.SetInt32(Session.UserIdKey, superAdmin.Id);

        return RedirectToPage("../HomePage/Index");
    }
}