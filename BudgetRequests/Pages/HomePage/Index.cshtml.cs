﻿using System.ComponentModel.DataAnnotations;
using BudgetRequests.Data;
using BudgetRequests.Models;
using BudgetRequests.Models.Admins;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BudgetRequests.Pages.HomePage;

public class IndexModel : PageModel
{
    private readonly DatabaseContext _context;

    public IndexModel(DatabaseContext context)
    {
        _context = context;
    }

    [BindProperty] public string Name { get; set; }

    public IActionResult OnGet()
    {
        var user = HttpContext.Session.GetLoggedInUser(_context);
        switch (user)
        {
            case null:
                return RedirectToPage("../Login/Index");
            default:
                Name = $"{user.FirstName} {user.LastName}";
                return Page();
        }
    }

    public IActionResult OnPostBudgetRequests()
    {
        return RedirectToPage("../Requests/Index");
    }    
    
    public IActionResult OnPostUsers()
    {
        return RedirectToPage("../Users/Index");
    }
    
    public IActionResult OnPostOrganizations()
    {
        return RedirectToPage("../Organizations/Index");
    }   
    
    public IActionResult OnPostProfile()
    {
        return RedirectToPage("../Profile/Index");
    }

    public IActionResult OnPostLogout()
    {
        HttpContext.Session.Logout();
        return RedirectToPage("../Index");
    }
}