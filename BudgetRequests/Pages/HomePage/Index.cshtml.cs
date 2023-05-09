using System.ComponentModel.DataAnnotations;
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

[Authorize]
public class IndexModel : PageModel
{
    private readonly DatabaseContext _context;

    public IndexModel(DatabaseContext context)
    {
        _context = context;
    }

    [BindProperty]
    public string Name { get; set; }

    public void OnGet()
    {
        var userId = HttpContext.Session.GetInt32(Session.UserIdKey);
        if (userId == null) return;
        var user = _context.GetUser((int)userId);
        if (user == null) return;
        Name = $"{user.FirstName} {user.LastName}";
    }
    
    public async Task<IActionResult> OnPostCreateAccount()
    {
        return RedirectToPage("../CreateUser/Index");
    }
    
    public async Task<IActionResult> OnPostLogout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
        
        HttpContext.Session.SetInt32(Session.UserIdKey, -1);

        return RedirectToPage("../Index");
    }
}