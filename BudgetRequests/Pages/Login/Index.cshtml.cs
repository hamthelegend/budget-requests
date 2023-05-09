﻿using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using BudgetRequests.Data;
using BudgetRequests.Models;
using BudgetRequests.Models.Admins;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetRequests.Pages.Login;

public class IndexModel : PageModel
{
    private readonly DatabaseContext _context;

    public IndexModel(DatabaseContext context)
    {
        _context = context;
    }
    
    [BindProperty]
    [Display(Name = "Username")]
    public string Username { get; set; }
    
    [BindProperty]
    [Display(Name = "Password")]
    public string Password { get; set; }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = _context.GetUsers().FirstOrDefault(x => x.Username == Username);
        var passwordHash = Password.ComputeHash(Convert.FromBase64String(user?.PasswordSalt ?? ""));

        if (user == null || user.PasswordHash != passwordHash) return Page();

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(Data.Login.GetClaimIdentity(user)),
            Data.Login.AuthProperties);

        HttpContext.Session.SetInt32(Session.UserIdKey, user.Id);

        return RedirectToPage("../HomePage/Index");

    }
}