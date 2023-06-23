using BudgetRequests.Data;
using BudgetRequests.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace BudgetRequests.Pages.Profile;

public class Index : PageModel
{
    private readonly BudgetRequests.Models.DatabaseContext _context;

    public Index(BudgetRequests.Models.DatabaseContext context)
    {
        _context = context;
    }
    
    public new User User { get; set; }

    public IActionResult OnGet()
    {
        var user = HttpContext.Session.GetLoggedInUser(_context);
        
        if (user == null)
        {
            return NotFound();
        }

        User = user;

        return Page();
    }
}