using BudgetRequests.Data;
using BudgetRequests.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetRequests.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    private readonly DatabaseContext _context;

    public IndexModel(ILogger<IndexModel> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult OnGet()
    {
        return RedirectToPage(!_context.HasSuperAdmin()
            ? "./Users/CreateSuperAdmin"
            : HttpContext.Session.IsLoggedIn()
                ? "./HomePage/Index"
                : "./Login/Index");
    }
}